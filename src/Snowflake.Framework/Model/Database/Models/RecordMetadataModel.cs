using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Snowflake.Model.Database.Models
{
    internal class RecordMetadataModel
    {
        public Guid RecordMetadataID { get; set; }
        public Guid RecordID { get; set; }
        public RecordModel? Record { get; set; }

        public string MetadataKey { get; set; }
        public string MetadataValue { get; set; } = "";

        internal static void SetupModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecordMetadataModel>()
                .HasOne(r => r.Record)
                .WithMany(r => r!.Metadata)
                .HasForeignKey(r => r.RecordID)
                .IsRequired();

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

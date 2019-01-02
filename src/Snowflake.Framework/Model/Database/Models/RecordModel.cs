using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Snowflake.Model.Database.Models
{
    internal class RecordModel
    {
        public Guid RecordID { get; set; }

        public string RecordType { get; set; }

        public List<RecordMetadataModel> Metadata { get; set; }

        internal static void SetupModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecordModel>()
                .HasKey(r => r.RecordID);

            modelBuilder.Entity<RecordModel>()
                .Property(r => r.RecordID)
                .IsRequired();

            modelBuilder.Entity<RecordModel>()
                .Property(r => r.RecordType)
                .IsRequired();
        }
    }
}

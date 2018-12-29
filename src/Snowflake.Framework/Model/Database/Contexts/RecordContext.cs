using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Snowflake.Model.Database.Contexts
{
    internal class RecordModel
    {
        public Guid RecordID { get; set; }

        public string RecordType { get; set; }

        public List<RecordMetadataModel> Metadata { get; set; }
    }

    internal class RecordContext : RecordMetadataContext
    {
        public RecordContext(DbContextOptions options)
           : base(options)
        {

        }

        public RecordContext(DbContextOptions<RecordContext> options)
          : base(options)
        {

        }

        public DbSet<RecordModel> Records { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<RecordModel>()
                .HasKey(r => r.RecordID);

            modelBuilder.Entity<RecordMetadataModel>()
                .HasOne(r => r.Record)
                .WithMany(r => r.Metadata);

            modelBuilder.Entity<RecordModel>()
                .Property(r => r.RecordID)
                .IsRequired();

            modelBuilder.Entity<RecordModel>()
                .Property(r => r.RecordType)
                .IsRequired();

            base.OnModelCreating(modelBuilder);

        }
    }
}

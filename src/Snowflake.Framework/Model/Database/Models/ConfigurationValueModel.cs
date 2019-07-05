using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using EnumsNET.NonGeneric;
using Microsoft.EntityFrameworkCore;

#nullable disable
namespace Snowflake.Model.Database.Models
{
    internal class ConfigurationValueModel
    {
        public Guid Guid { get; set; }
        public string Value { get; set; }

        public string SectionKey { get; set; }
        public string OptionKey { get; set; }

        public Guid ValueCollectionGuid { get; set; }
        public ConfigurationProfileModel Profile { get; set; }

        internal static void SetupModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfigurationValueModel>()
                .HasOne(p => p.Profile)
                .WithMany(p => p!.Values)
                .HasForeignKey(p => p.ValueCollectionGuid)
                .IsRequired();

            modelBuilder.Entity<ConfigurationValueModel>()
                .Property(p => p.Guid)
                .IsRequired();

            modelBuilder.Entity<ConfigurationValueModel>()
                .Property(p => p.SectionKey)
                .IsRequired();

            modelBuilder.Entity<ConfigurationValueModel>()
                .Property(p => p.OptionKey)
                .IsRequired();

            modelBuilder.Entity<ConfigurationValueModel>()
                .Property(p => p.ValueCollectionGuid)
                .IsRequired();

            modelBuilder.Entity<ConfigurationValueModel>()
                .Property(p => p.Value)
                .IsRequired();

            modelBuilder.Entity<ConfigurationValueModel>()
                .HasKey(p => p.Guid);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

#nullable disable
namespace Snowflake.Model.Database.Models
{
    internal class ConfigurationProfileModel
    {
        public string ConfigurationSource { get; set; }

        public Guid ValueCollectionGuid { get; set; }

        public List<ConfigurationValueModel> Values { get; set; }

        public static void SetupModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfigurationProfileModel>()
                .HasKey(p => p.ValueCollectionGuid);
        }
    }
}

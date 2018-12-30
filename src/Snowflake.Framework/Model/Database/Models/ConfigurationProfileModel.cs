using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Snowflake.Model.Database.Models
{
    internal class ConfigurationProfileModel
    {
        public string CollectionTypeName { get; set; }
     
        public Guid ValueCollectionGuid { get; set; }

        public List<ConfigurationValueModel> Values { get; set; }

        public static void SetupModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfigurationProfileModel>()
                .HasKey(p => p.ValueCollectionGuid);

            //modelBuilder.Entity<ConfigurationProfileModel>()
            //    .HasOne(p => p.Game)
            //    .WithMany()
            //    .HasForeignKey(p => p.GameRecordGuid)
            //    .IsRequired(false);
 
        }
    }
}

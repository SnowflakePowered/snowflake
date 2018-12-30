using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Snowflake.Model.Database.Models
{
    internal class ControllerElementMappingsModel
    {
        public List<MappedControllerElementModel> MappedElements { get; set; }

        public string ControllerID { get; set; }

        public string DeviceID { get; set; }

        public string ProfileName { get; set; }

        internal static void SetupModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ControllerElementMappingsModel>()
                .Property(p => p.ControllerID)
                .IsRequired();

            modelBuilder.Entity<ControllerElementMappingsModel>()
               .Property(p => p.DeviceID)
               .IsRequired();

            modelBuilder.Entity<ControllerElementMappingsModel>()
               .HasKey(p => new { p.ControllerID, p.DeviceID, p.ProfileName});
        }
    }
}


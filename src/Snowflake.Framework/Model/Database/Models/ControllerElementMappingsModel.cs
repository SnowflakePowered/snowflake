using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;

#nullable disable
namespace Snowflake.Model.Database.Models
{
    internal class ControllerElementMappingsModel
    {
        public List<MappedControllerElementModel> MappedElements { get; set; }

        public ControllerId ControllerID { get; set; }

        public string DeviceName { get; set; }

        public string ProfileName { get; set; }

        public int VendorID { get; set; }

        public InputDriverType DriverType { get; set; }
        internal static void SetupModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ControllerElementMappingsModel>()
                .Property(p => p.ControllerID)
                .HasConversion(p => p.ToString(), s => s)
                .IsRequired();

            modelBuilder.Entity<ControllerElementMappingsModel>()
                .Property(p => p.DeviceName)
                .IsRequired();

            modelBuilder.Entity<ControllerElementMappingsModel>()
                .Property(p => p.VendorID)
                .IsRequired();

            modelBuilder.Entity<ControllerElementMappingsModel>()
               .Property(p => p.DriverType)
               .IsRequired();

            modelBuilder.Entity<ControllerElementMappingsModel>()
                .HasKey(p => new {p.ControllerID, p.DeviceName, p.DriverType, p.ProfileName});
        }
    }
}

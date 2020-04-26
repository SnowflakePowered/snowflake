using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;

#nullable disable
namespace Snowflake.Model.Database.Models
{
    internal class ControllerElementMappingCollectionModel
    {
        public List<ControllerElementMappingModel> MappedElements { get; set; }

        public ControllerId ControllerID { get; set; }

        public string DeviceName { get; set; }

        public string ProfileName { get; set; }

        public Guid ProfileID { get; set; }

        public int VendorID { get; set; }

        public InputDriver DriverType { get; set; }
        internal static void SetupModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ControllerElementMappingCollectionModel>()
                .Property(p => p.ControllerID)
                .HasConversion(p => p.ToString(), s => s)
                .IsRequired();

            modelBuilder.Entity<ControllerElementMappingCollectionModel>()
                .Property(p => p.DeviceName)
                .IsRequired();

            modelBuilder.Entity<ControllerElementMappingCollectionModel>()
                .Property(p => p.VendorID)
                .IsRequired();

            modelBuilder.Entity<ControllerElementMappingCollectionModel>()
               .Property(p => p.DriverType)
               .IsRequired();

            modelBuilder.Entity<ControllerElementMappingCollectionModel>()
                .HasKey(p => p.ProfileID);
        }
    }
}

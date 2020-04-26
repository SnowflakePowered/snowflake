using System;
using System.Collections.Generic;
using System.Text;
using EnumsNET;
using Microsoft.EntityFrameworkCore;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;

#nullable disable
namespace Snowflake.Model.Database.Models
{
    internal class ControllerElementMappingModel
    {
        public ControllerElement LayoutElement { get; set; }
        public DeviceCapability DeviceCapability { get; set; }
        public Guid ProfileID { get; set; }

        public ControllerElementMappingCollectionModel Collection { get; set; }

        internal static void SetupModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ControllerElementMappingModel>()
                .Property(p => p.LayoutElement)
                .HasConversion(e => Enums.AsString(e), e => Enums.Parse<ControllerElement>(e))
                .IsRequired();

            modelBuilder.Entity<ControllerElementMappingModel>()
                .Property(p => p.DeviceCapability)
                .HasConversion(e => Enums.AsString(e), e => Enums.Parse<DeviceCapability>(e))
                .IsRequired();

            modelBuilder.Entity<ControllerElementMappingModel>()
                .HasOne(e => e.Collection)
                .WithMany(e => e!.MappedElements)
                .HasForeignKey(p => p.ProfileID);

            modelBuilder.Entity<ControllerElementMappingModel>()
                .HasKey(p => new {p.ProfileID, p.LayoutElement});
        }
    }
}

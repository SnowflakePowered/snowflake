using System;
using System.Collections.Generic;
using System.Text;
using EnumsNET;
using Microsoft.EntityFrameworkCore;
using Snowflake.Input.Controller;

namespace Snowflake.Model.Database.Models
{
    internal class MappedControllerElementModel
    {
        public ControllerElement LayoutElement { get; set; }
        public ControllerElement DeviceElement { get; set; }

        public string ControllerID { get; set; }
        public string DeviceID { get; set; }
        public string ProfileName { get; set; }

        public ControllerElementMappingsModel? Collection { get; set; }

        internal static void SetupModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MappedControllerElementModel>()
                .Property(p => p.LayoutElement)
                .HasConversion(e => Enums.AsString(e), e => Enums.Parse<ControllerElement>(e))
                .IsRequired();

            modelBuilder.Entity<MappedControllerElementModel>()
                .Property(p => p.DeviceElement)
                .HasConversion(e => Enums.AsString(e), e => Enums.Parse<ControllerElement>(e))
                .IsRequired();

            modelBuilder.Entity<MappedControllerElementModel>()
                .HasOne(e => e.Collection)
                .WithMany(e => e.MappedElements)
                .HasForeignKey(p => new {p.ControllerID, p.DeviceID, p.ProfileName});

            modelBuilder.Entity<MappedControllerElementModel>()
                .HasKey(p => new {p.ControllerID, p.DeviceID, p.ProfileName, p.LayoutElement});
        }
    }
}

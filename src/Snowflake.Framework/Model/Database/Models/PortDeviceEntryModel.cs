﻿using Microsoft.EntityFrameworkCore;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using Snowflake.Model.Game;
using Snowflake.Orchestration.Extensibility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Model.Database.Models
{
    internal class PortDeviceEntryModel : IEmulatedPortDeviceEntry
    {
        public InputDriverType Driver { get; set; }
        public ControllerId ControllerID { get; set; }
        public PlatformId PlatformID { get; set; }
        public string DeviceName { get; set; }
        public string ProfileName { get; set; }
        public int UniqueNameEnumerationIndex { get; set; }
        public int PortIndex { get; set; }
        public string OrchestratorName { get; set; }

        internal static void SetupModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PortDeviceEntryModel>()
                .Property(p => p.Driver)
                .IsRequired();
            modelBuilder.Entity<PortDeviceEntryModel>()
              .Property(r => r.PlatformID)
              .HasConversion(p => p.ToString(), s => s)
              .IsRequired();
            modelBuilder.Entity<PortDeviceEntryModel>()
             .Property(p => p.ControllerID)
             .HasConversion(p => p.ToString(), s => s)
             .IsRequired();
            modelBuilder.Entity<PortDeviceEntryModel>()
               .Property(p => p.DeviceName)
               .IsRequired();
            modelBuilder.Entity<PortDeviceEntryModel>()
               .Property(p => p.ProfileName)
               .IsRequired();
            modelBuilder.Entity<PortDeviceEntryModel>()
               .Property(p => p.PortIndex)
               .IsRequired();
            modelBuilder.Entity<PortDeviceEntryModel>()
               .Property(p => p.UniqueNameEnumerationIndex)
               .IsRequired();
            modelBuilder.Entity<PortDeviceEntryModel>()
               .Property(p => p.OrchestratorName)
               .IsRequired();
            modelBuilder.Entity<PortDeviceEntryModel>()
                .HasKey(p => new { p.OrchestratorName, p.PlatformID, p.PortIndex });
        }
    }
}

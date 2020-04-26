using Microsoft.EntityFrameworkCore;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using Snowflake.Model.Game;
using Snowflake.Orchestration.Extensibility;
using System;

namespace Snowflake.Model.Database.Models
{
    internal class PortDeviceEntryModel : IEmulatedPortDeviceEntry
    {
        public InputDriver Driver { get; set; }
        public ControllerId ControllerID { get; set; }
        public PlatformId PlatformID { get; set; }
        public Guid InstanceGuid { get; set; }
        public  Guid ProfileGuid { get; set; }
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
               .Property(p => p.InstanceGuid)
               .IsRequired();
            modelBuilder.Entity<PortDeviceEntryModel>()
               .Property(p => p.ProfileGuid)
               .IsRequired();
            modelBuilder.Entity<PortDeviceEntryModel>()
               .Property(p => p.PortIndex)
               .IsRequired();
            modelBuilder.Entity<PortDeviceEntryModel>()
               .Property(p => p.OrchestratorName)
               .IsRequired();
            modelBuilder.Entity<PortDeviceEntryModel>()
                .HasKey(p => new { p.OrchestratorName, p.PlatformID, p.PortIndex });
        }
    }
}

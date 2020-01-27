using Microsoft.EntityFrameworkCore;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using Snowflake.Model.Database.Models;
using Snowflake.Model.Game;
using Snowflake.Orchestration.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Model.Database
{
    internal class EmulatedPortStore : IEmulatedPortStore
    {
        private DbContextOptionsBuilder<DatabaseContext> Options { get; set; }

        public EmulatedPortStore(DbContextOptionsBuilder<DatabaseContext> options)
        {
            this.Options = options;
            using var context = new DatabaseContext(Options.Options);
            context.Database.Migrate();
        }

        public IEmulatedPortDeviceEntry? GetPort(PlatformId platform, int portNumber, string emulatorName)
        {
            using var context = new DatabaseContext(Options.Options);
            var entity = context.PortDeviceEntries
                .Find(emulatorName, platform, portNumber);
            return entity;
        }

        public IEnumerable<IEmulatedPortDeviceEntry> GetPortsForPlatform(PlatformId platform, string emulatorName)
        {
            using var context = new DatabaseContext(Options.Options);
            var entity = context.PortDeviceEntries
                .Where(p => p.OrchestratorName == emulatorName && p.PlatformID == platform)
                .ToList();
            return entity;
        }

        public void ClearPort(PlatformId platform, int portNumber, string emulatorName)
        {
            using var context = new DatabaseContext(Options.Options);
            var entity = context.PortDeviceEntries
                .Find(emulatorName, platform, portNumber);
            if (entity != null)
            {
                context.Entry(entity).State = EntityState.Deleted;
            }
            context.SaveChanges();
        }

        public void SetPort(PlatformId platform, int portNumber, ControllerId controller, 
            IInputDevice device, IInputDeviceInstance instance, string inputProfile, string emulatorName)
        {
            using var context = new DatabaseContext(Options.Options);
            var entity = context.PortDeviceEntries
                .Find(emulatorName, platform, portNumber);
            if (entity != null)
            {
                entity.ProfileName = inputProfile;
                entity.ProductEnumerationIndex = instance.NameEnumerationIndex;
                entity.ControllerID = controller;
                entity.DeviceName = device.DeviceName;
                entity.Driver = instance.Driver;
                entity.VendorID = device.VendorID;
                context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                var newEntity = new PortDeviceEntryModel()
                {
                    ProfileName = inputProfile,
                    ProductEnumerationIndex = instance.NameEnumerationIndex,
                    ControllerID = controller,
                    DeviceName = device.DeviceName,
                    Driver = instance.Driver,
                    OrchestratorName = emulatorName,
                    PlatformID = platform,
                    VendorID = device.VendorID,
                    PortIndex = portNumber
                };
                context.PortDeviceEntries.Add(newEntity);
            }
            context.SaveChanges();
        }

        public IEmulatedPortDeviceEntry? GetPort(IEmulatorOrchestrator orchestrator, PlatformId platform, int portNumber)
            => this.GetPort(platform, portNumber, orchestrator.Name);
        public void SetPort(IEmulatorOrchestrator orchestrator, PlatformId platform, int portNumber,
            ControllerId controller, IInputDevice device, IInputDeviceInstance instance, string inputProfile)
            => this.SetPort(platform, portNumber, controller, device, instance, inputProfile, orchestrator.Name);
        public void ClearPort(IEmulatorOrchestrator orchestrator, PlatformId platform, int portNumber)
            => this.ClearPort(platform, portNumber, orchestrator.Name);
        public IEnumerable<IEmulatedPortDeviceEntry> EnumeratePorts(IEmulatorOrchestrator orchestrator, PlatformId platform)
            => this.GetPortsForPlatform(platform, orchestrator.Name);
    }
}

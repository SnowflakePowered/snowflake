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
    internal class EmulatedPortStoreProvider : IEmulatedPortStoreProvider
    {
        private DbContextOptionsBuilder<DatabaseContext> Options { get; set; }

        public EmulatedPortStoreProvider(DbContextOptionsBuilder<DatabaseContext> options)
        {
            this.Options = options;
            using var context = new DatabaseContext(Options.Options);
            context.Database.Migrate();
        }

        public IEmulatedPortDeviceEntry? GetPort(PlatformId platform, int portNumber, string emulatorName)
        {
            using var context = new DatabaseContext(Options.Options);
            var entity = context.PortDeviceEntries
                .Find(new { OrchestratorName = emulatorName, PlatformID = platform, PortIndex = portNumber });
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
                .Find(new { OrchestratorName = emulatorName, PlatformID = platform, PortIndex = portNumber });
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
                .Find(new { OrchestratorName = emulatorName, PlatformID = platform, PortIndex = portNumber });
            if (entity != null)
            {
                entity.ProfileName = inputProfile;
                entity.UniqueNameEnumerationIndex = instance.UniqueNameEnumerationIndex;
                entity.ControllerID = controller;
                entity.DeviceName = device.DeviceName;
                entity.Driver = instance.Driver;
                context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                var newEntity = new PortDeviceEntryModel()
                {
                    ProfileName = inputProfile,
                    UniqueNameEnumerationIndex = instance.UniqueNameEnumerationIndex,
                    ControllerID = controller,
                    DeviceName = device.DeviceName,
                    Driver = instance.Driver,
                    OrchestratorName = emulatorName,
                    PlatformID = platform,
                    PortIndex = portNumber
                };
                context.PortDeviceEntries.Add(newEntity);
            }
            context.SaveChanges();
        }

        public IEmulatedPortStore GetPortStore(IEmulatorOrchestrator orchestrator)
        {
            throw new NotImplementedException();
        }
    }
}

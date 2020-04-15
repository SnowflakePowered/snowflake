using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Model.Database.Extensions;
using Snowflake.Model.Database.Models;

namespace Snowflake.Model.Database
{
    internal partial class ControllerElementMappingsStore : IControllerElementMappingsStore
    {
        private DbContextOptionsBuilder<DatabaseContext> Options { get; set; }

        public ControllerElementMappingsStore(DbContextOptionsBuilder<DatabaseContext> options)
        {
            this.Options = options;
            using var context = new DatabaseContext(this.Options.Options);
            context.Database.Migrate();
        }

        #region Synchronous API
        public void AddMappings(IControllerElementMappingCollection mappings, string profileName)
        {
            using var context = new DatabaseContext(this.Options.Options);
            // todo: check already exists
            context.ControllerElementMappings.Add(mappings.AsModel(profileName));
            context.SaveChanges();
        }

        public IControllerElementMappingCollection? GetMappings(ControllerId controllerId,
            InputDriver driver,
            string deviceName,
            int vendorId,
            string profileName
            )
        {
            using var context = new DatabaseContext(this.Options.Options);
            var mappings = context.ControllerElementMappings
                .Include(p => p.MappedElements)
                .SingleOrDefault(p => p.ControllerID == controllerId
                                    && p.DriverType == driver
                                    && p.DeviceName == deviceName
                                    && p.VendorID == vendorId
                                    && p.ProfileName == profileName);
            return mappings?.AsControllerElementMappings();
        }

        public IQueryable<IControllerElementMappingCollection> GetMappings(ControllerId controllerId,
            string deviceName, int vendorId)
        {
            var context = new DatabaseContext(this.Options.Options);
            var mappings = context.ControllerElementMappings
                 .Where(p => p.ControllerID == controllerId
                         && p.DeviceName == deviceName
                         && p.VendorID == vendorId)
                .Include(p => p.MappedElements)
                .Select(m => m.AsControllerElementMappings());
            return mappings;
        }

        public void DeleteMappings(ControllerId controllerId, string deviceName, int vendorId)
        {
            using var context = new DatabaseContext(this.Options.Options);
            var retrievedMappings = context.ControllerElementMappings
                .Where(p => p.ControllerID == controllerId
                         && p.DeviceName == deviceName
                         && p.VendorID == vendorId)
                .Include(p => p.MappedElements);
               

            foreach (var retrievedMapping in retrievedMappings)
            {
                context.Entry(retrievedMapping).State = EntityState.Deleted;
            }

            context.SaveChanges();
        }

        public void UpdateMappings(IControllerElementMappingCollection mappings, string profileName)
        {
            using var context = new DatabaseContext(this.Options.Options);
            var retrievedMappings = context.ControllerElementMappings
                .Include(p => p.MappedElements)
                .SingleOrDefault(p => p.ControllerID == mappings.ControllerID 
                                   && p.DriverType == mappings.DriverType
                                   && p.DeviceName == mappings.DeviceName 
                                   && p.VendorID == mappings.VendorID
                                   && p.ProfileName == profileName);

            foreach (var mapping in retrievedMappings.MappedElements)
            {
                if (mappings[mapping.LayoutElement] == mapping.DeviceCapability) continue;
                mapping.DeviceCapability = mappings[mapping.LayoutElement];
                context.Entry(mapping).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        public void DeleteMappings(ControllerId controllerId, InputDriver driverType,
            string deviceName, int vendorId, string profileName)
        {
            using var context = new DatabaseContext(this.Options.Options);
            var retrievedMappings = context.ControllerElementMappings
                .Include(p => p.MappedElements)
                .SingleOrDefault(p => p.ControllerID == controllerId 
                                    && p.DriverType == driverType
                                    && p.DeviceName == deviceName
                                    && p.VendorID == vendorId
                                    && p.ProfileName == profileName);

            if (retrievedMappings != null)
            {
                context.Entry(retrievedMappings).State = EntityState.Deleted;
            }

            context.SaveChanges();
        }
        #endregion
    }
}

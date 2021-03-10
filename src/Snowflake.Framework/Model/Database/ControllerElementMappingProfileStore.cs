using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using Snowflake.Model.Database.Extensions;
using Snowflake.Model.Database.Models;

namespace Snowflake.Model.Database
{
    internal partial class ControllerElementMappingProfileStore : IControllerElementMappingProfileStore
    {
        private DbContextOptionsBuilder<DatabaseContext> Options { get; set; }

        public ControllerElementMappingProfileStore(DbContextOptionsBuilder<DatabaseContext> options)
        {
            this.Options = options;
            using var context = new DatabaseContext(this.Options.Options);
            context.Database.Migrate();
        }

        #region Synchronous API
        public void AddMappings(IControllerElementMappingProfile mappings, string profileName)
        {
            using var context = new DatabaseContext(this.Options.Options);
            // todo: check already exists
            context.ControllerElementMappings.Add(mappings.AsModel(profileName));
            context.SaveChanges();
        }

        public IControllerElementMappingProfile? GetMappings(Guid profileGuid)
        {
            using var context = new DatabaseContext(this.Options.Options);
            var mappings = context.ControllerElementMappings
                .Include(p => p.MappedElements)
                .SingleOrDefault(p => p.ProfileID == profileGuid);
            return mappings?.AsControllerElementMappings();
        }

        public void DeleteMappings(ControllerId controllerId, string deviceName, int vendorId)
        {
            using var context = new DatabaseContext(this.Options.Options);
            var retrievedMappings = context.ControllerElementMappings
                .Where(p => p.ControllerID == controllerId
                         && p.DeviceName == deviceName
                         && p.VendorID == vendorId);

            foreach (var retrievedMapping in retrievedMappings)
            {
                context.Entry(retrievedMapping).State = EntityState.Deleted;
            }

            context.SaveChanges();
        }

        public void UpdateMappings(IControllerElementMappingProfile mappings)
        {
            using var context = new DatabaseContext(this.Options.Options);
            var retrievedMappings = context.ControllerElementMappings
                .Include(p => p.MappedElements)
                .SingleOrDefault(p => p.ProfileID == mappings.ProfileGuid);

            if (retrievedMappings == null)
                return;

            var mappingSet = mappings.Select(k => k.LayoutElement).ToHashSet();
            foreach (var mapping in retrievedMappings.MappedElements)
            {
                try
                {
                    if (mappings[mapping.LayoutElement] == mapping.DeviceCapability) continue;
                    mapping.DeviceCapability = mappings[mapping.LayoutElement];
                    context.Entry(mapping).State = EntityState.Modified;
                } 
                finally
                {
                    mappingSet.Remove(mapping.LayoutElement);
                }
            }

            foreach (var toChange in mappingSet)
            {
                var model = new ControllerElementMappingModel
                {
                    LayoutElement = toChange,
                    DeviceCapability = mappings[toChange],
                    Collection = retrievedMappings,
                    ProfileID = retrievedMappings.ProfileID
                };
                retrievedMappings.MappedElements.Add(model);
                context.Entry(model).State = EntityState.Added;
            }

            context.SaveChanges();
        }

        public void DeleteMappings(Guid profileGuid)
        {
            using var context = new DatabaseContext(this.Options.Options);
            var mappings = context.ControllerElementMappings.Find(profileGuid);
            if (mappings != null)
            {
                context.Entry(mappings).State = EntityState.Deleted;
            }

            context.SaveChanges();
        }

        public IEnumerable<(string profileName, Guid profileGuid)> GetProfileNames(ControllerId controllerId, InputDriver driverType, string deviceName, int vendorId)
        {
            using var context = new DatabaseContext(this.Options.Options);
            var retrievedMappings = context.ControllerElementMappings
                .Where(p => p.ControllerID == controllerId
                                    && p.DriverType == driverType
                                    && p.DeviceName == deviceName
                                    && p.VendorID == vendorId)
                .ToList()
                .Select(p => (p.ProfileName, p.ProfileID));
            return retrievedMappings;
        }
        #endregion
    }
}

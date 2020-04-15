using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        #region Asynchronous API
        public async Task AddMappingsAsync(IControllerElementMappingCollection mappings, string profileName)
        {
            await using var context = new DatabaseContext(this.Options.Options);
            // todo: check already exists
            await context.ControllerElementMappings.AddAsync(mappings.AsModel(profileName));
            await context.SaveChangesAsync();
        }

        public async Task<IControllerElementMappingCollection?> GetMappingsAsync(ControllerId controllerId,
            InputDriver driver,
            string deviceName,
            int vendorId,
            string profileName
            )
        {
            await using var context = new DatabaseContext(this.Options.Options);
            var mappings = await context.ControllerElementMappings
                .Include(p => p.MappedElements)
                .SingleOrDefaultAsync(p => p.ControllerID == controllerId
                                    && p.DriverType == driver
                                    && p.DeviceName == deviceName
                                    && p.VendorID == vendorId
                                    && p.ProfileName == profileName);
            return mappings?.AsControllerElementMappings();
        }

        public IAsyncEnumerable<IControllerElementMappingCollection> GetMappingsAsync(ControllerId controllerId,
            string deviceName, int vendorId)
        {
            var context = new DatabaseContext(this.Options.Options);
            return context.ControllerElementMappings
                 .Where(p => p.ControllerID == controllerId
                         && p.DeviceName == deviceName
                         && p.VendorID == vendorId)
                .Include(p => p.MappedElements)
                .Select(m => m.AsControllerElementMappings())
                .AsAsyncEnumerable();
        }

        public async Task DeleteMappingsAsync(ControllerId controllerId, string deviceName, int vendorId)
        {
            await using var context = new DatabaseContext(this.Options.Options);
            var retrievedMappings = context.ControllerElementMappings
                .Where(p => p.ControllerID == controllerId
                         && p.DeviceName == deviceName
                         && p.VendorID == vendorId)
                .Include(p => p.MappedElements);


            foreach (var retrievedMapping in retrievedMappings)
            {
                context.Entry(retrievedMapping).State = EntityState.Deleted;
            }

            await context.SaveChangesAsync();
        }

        public async Task UpdateMappingsAsync(IControllerElementMappingCollection mappings, string profileName)
        {
            await using var context = new DatabaseContext(this.Options.Options);
            var retrievedMappings = await context.ControllerElementMappings
                .Include(p => p.MappedElements)
                .SingleOrDefaultAsync(p => p.ControllerID == mappings.ControllerID
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

            await context.SaveChangesAsync();
        }

        public async Task DeleteMappingsAsync(ControllerId controllerId, InputDriver driverType,
            string deviceName, int vendorId, string profileName)
        {
            await using var context = new DatabaseContext(this.Options.Options);
            var retrievedMappings = await context.ControllerElementMappings
                .Include(p => p.MappedElements)
                .SingleOrDefaultAsync(p => p.ControllerID == controllerId
                                    && p.DriverType == driverType
                                    && p.DeviceName == deviceName
                                    && p.VendorID == vendorId
                                    && p.ProfileName == profileName);

            if (retrievedMappings != null)
            {
                context.Entry(retrievedMappings).State = EntityState.Deleted;
            }

            await context.SaveChangesAsync();
        }
        #endregion
    }
}

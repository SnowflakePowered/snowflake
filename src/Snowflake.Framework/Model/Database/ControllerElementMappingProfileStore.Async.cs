using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Snowflake.Input.Controller;
using Snowflake.Model.Database.Extensions;
using Snowflake.Model.Database.Models;

namespace Snowflake.Model.Database
{
    internal partial class ControllerElementMappingProfileStore : IControllerElementMappingProfileStore
    {
        #region Asynchronous API
        public async Task AddMappingsAsync(IControllerElementMappingProfile mappings, string profileName)
        {
            await using var context = new DatabaseContext(this.Options.Options);
            // todo: check already exists
            await context.ControllerElementMappings.AddAsync(mappings.AsModel(profileName));
            await context.SaveChangesAsync();
        }

        public async Task<IControllerElementMappingProfile?> GetMappingsAsync(Guid profileGuid)
        {
            await using var context = new DatabaseContext(this.Options.Options);
            var mappings = await context.ControllerElementMappings
                .Include(p => p.MappedElements)
                .SingleOrDefaultAsync(p => p.ProfileID == profileGuid);
            return mappings?.AsControllerElementMappings();
        }

        public async Task DeleteMappingsAsync(ControllerId controllerId, string deviceName, int vendorId)
        {
            await using var context = new DatabaseContext(this.Options.Options);
            var retrievedMappings = context.ControllerElementMappings
                .Where(p => p.ControllerID == controllerId
                         && p.DeviceName == deviceName
                         && p.VendorID == vendorId);


            foreach (var retrievedMapping in retrievedMappings)
            {
                context.Entry(retrievedMapping).State = EntityState.Deleted;
            }

            await context.SaveChangesAsync();
        }

        public async Task UpdateMappingsAsync(IControllerElementMappingProfile mappings)
        {
            await using var context = new DatabaseContext(this.Options.Options);
            var retrievedMappings = await context.ControllerElementMappings
                .Include(p => p.MappedElements)
                .SingleOrDefaultAsync(p => p.ProfileID == mappings.ProfileGuid);

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

            await context.SaveChangesAsync();
        }

        public async Task DeleteMappingsAsync(Guid profileGuid)
        {
            await using var context = new DatabaseContext(this.Options.Options);
            var retrievedMappings = await context.ControllerElementMappings.FindAsync(profileGuid);

            if (retrievedMappings != null)
            {
                context.Entry(retrievedMappings).State = EntityState.Deleted;
            }

            await context.SaveChangesAsync();
        }
        #endregion
    }
}

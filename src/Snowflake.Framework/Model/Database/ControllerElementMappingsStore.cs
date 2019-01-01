using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Model.Database.Extensions;
using Snowflake.Model.Database.Models;

namespace Snowflake.Model.Database
{
    internal class ControllerElementMappingsStore : IControllerElementMappingsStore
    {
        private DbContextOptionsBuilder<DatabaseContext> Options { get; set; }

        public ControllerElementMappingsStore(DbContextOptionsBuilder<DatabaseContext> options)
        {
            this.Options = options;
            using (var context = new DatabaseContext(this.Options.Options))
            {
                context.Database.EnsureCreated();
            }
        }

        public void AddMappings(IControllerElementMappings mappings, string profileName)
        {
            using (var context = new DatabaseContext(this.Options.Options))
            {
                context.ControllerElementMappings.Add(mappings.AsModel(profileName));
                context.SaveChanges();
            }
        }

        public IControllerElementMappings? GetMappings(string controllerId, string deviceId, string profileName)
        {
            using (var context = new DatabaseContext(this.Options.Options))
            {
                var mappings = context.ControllerElementMappings
                    .Include(p => p.MappedElements)
                    .SingleOrDefault(p => p.ControllerID == controllerId &&
                    p.DeviceID == deviceId && p.ProfileName == profileName);
                return mappings?.AsControllerElementMappings();
            }
        }

        public IEnumerable<IControllerElementMappings> GetMappings(string controllerId, string deviceId)
        {
            using (var context = new DatabaseContext(this.Options.Options))
            {
                var mappings = context.ControllerElementMappings
                    .Include(p => p.MappedElements)
                    .Where(p => p.ControllerID == controllerId &&
                            p.DeviceID == deviceId)
                    .Select(m => m.AsControllerElementMappings())
                    .ToList();
                return mappings;
            }
        }


        public void DeleteMappings(string controllerId, string deviceId)
        {
            using (var context = new DatabaseContext(this.Options.Options))
            {
                var retrievedMappings = context.ControllerElementMappings
                .Include(p => p.MappedElements)
                .Where(p => p.ControllerID == controllerId &&
                    p.DeviceID == deviceId);
                
                foreach (var retrievedMapping in retrievedMappings)
                {
                    context.Entry(retrievedMapping).State = EntityState.Deleted;
                }
 
                context.SaveChanges();
            }
        }

        public void UpdateMappings(IControllerElementMappings mappings, string profileName)
        {
            using (var context = new DatabaseContext(this.Options.Options))
            {
                var retrievedMappings = context.ControllerElementMappings
                    .Include(p => p.MappedElements)
                    .SingleOrDefault(p => p.ControllerID == mappings.ControllerId &&
                    p.DeviceID == mappings.DeviceId && p.ProfileName == profileName);

                foreach (var mapping in retrievedMappings.MappedElements)
                {
                    if (mappings[mapping.LayoutElement] != mapping.DeviceElement)
                    {
                        mapping.DeviceElement = mappings[mapping.LayoutElement];
                        context.Entry(mapping).State = EntityState.Modified;
                    }
                }

                context.SaveChanges();
            }
        }

        public void DeleteMappings(string controllerId, string deviceId, string profileName)
        {
            using (var context = new DatabaseContext(this.Options.Options))
            {
                var retrievedMappings = context.ControllerElementMappings
                .Include(p => p.MappedElements)
                .SingleOrDefault(p => p.ControllerID == controllerId &&
                    p.DeviceID == deviceId && p.ProfileName == profileName);

                if (retrievedMappings != null)
                {
                    context.Entry(retrievedMappings).State = EntityState.Deleted;
                }

                context.SaveChanges();
            }
        }
    }
}

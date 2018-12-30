using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Model.Database.Extensions;
using Snowflake.Model.Database.Models;

namespace Snowflake.Model.Database
{
    internal class ControllerElementMappingsStore
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
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Snowflake.Model.Database.Models;

namespace Snowflake.Support.StoreProviders
{
   internal class DesignTimeSqliteDbContextFactory :
        IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder
                .UseSqlite($"Data Source=library.db");
            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}

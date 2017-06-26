using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Persistence;

namespace Snowflake.Services
{
    public interface ISqliteDatabaseProvider
    {
        ISqlDatabase CreateDatabase(string databaseName);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Persistence;
using System.IO;

namespace Snowflake.Services.Persistence
{
    public class SqliteDatabaseProvider : ISqliteDatabaseProvider
    {
        private readonly DirectoryInfo databaseRoot;
        public SqliteDatabaseProvider(DirectoryInfo databaseRoot)
        {
            this.databaseRoot = databaseRoot;
        }

        public ISqlDatabase CreateDatabase(string databaseName)
        {
            return new SqliteDatabase(Path.Combine(databaseRoot.FullName, $"{databaseName}.db"));
        }
    }
}

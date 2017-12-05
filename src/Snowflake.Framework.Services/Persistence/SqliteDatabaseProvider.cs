using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Snowflake.Persistence;

namespace Snowflake.Services.Persistence
{
    public class SqliteDatabaseProvider : ISqliteDatabaseProvider
    {
        private readonly DirectoryInfo databaseRoot;
        public SqliteDatabaseProvider(DirectoryInfo databaseRoot)
        {
            this.databaseRoot = databaseRoot;
        }

        /// <inheritdoc/>
        public ISqlDatabase CreateDatabase(string databaseName)
        {
            return new SqliteDatabase(Path.Combine(databaseRoot.FullName, $"{databaseName}.db"));
        }

        /// <inheritdoc/>
        public ISqlDatabase CreateDatabase(string universe, string databaseName)
        {
            return new SqliteDatabase(Path.Combine(databaseRoot.CreateSubdirectory(universe).FullName, $"{databaseName}.db"));
        }
    }
}

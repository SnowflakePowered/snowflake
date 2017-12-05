using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Persistence;

namespace Snowflake.Services
{
    /// <summary>
    /// Providers access to creation of databases
    /// </summary>
    public interface ISqliteDatabaseProvider
    {
        /// <summary>
        /// Creates a database under the root universe.
        /// </summary>
        /// <param name="databaseName">The name of the database</param>
        /// <returns>A SQLite database instance</returns>
        ISqlDatabase CreateDatabase(string databaseName);

        /// <summary>
        /// Creates a database under the specified universe.
        /// </summary>
        /// <param name="universe">The universe to create the database under</param>
        /// <param name="databaseName">The name of the database</param>
        /// <returns>A SQLite database instance</returns>
        ISqlDatabase CreateDatabase(string universe, string databaseName);
    }
}

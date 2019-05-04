using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Snowflake.Persistence
{
    public class SqliteMemoryDatabase
    {
        private readonly SqliteConnection dbConnection;

        public SqliteMemoryDatabase()
        {
            this.dbConnection = new SqliteConnection("Data Source=:memory:;");
            this.dbConnection.Open();
        }

        /// <summary>
        /// Queries on a new connection to the database.
        /// The connection will be safely closed after the operation.
        /// </summary>
        /// <typeparam name="T">The type the query will return</typeparam>
        /// <param name="queryFunction">A function to query the database using the opened connection</param>
        /// <returns>The requested data.</returns>
        public T Query<T>(Func<DbConnection, T> queryFunction)
        {
            return queryFunction(this.dbConnection);
        }

        /// <summary>
        /// Executes on a new connection to the database.
        /// The connection will be safely closed after the operation.
        /// </summary>
        /// <param name="queryFunction">A function to query the database using the opened connection</param>
        public void Execute(Action<DbConnection> queryFunction)
        {
            queryFunction(this.dbConnection);
        }

        /// <summary>
        /// Queries on a new connection to the database.
        /// The connection will be safely closed after the operation.
        /// </summary>
        /// <typeparam name="T">The type the query will return</typeparam>
        /// <param name="query">The SQL query to execute</param>
        /// <param name="param">The query parameters</param>
        /// <returns>The requested data.</returns>
        public IEnumerable<T> Query<T>(string query, object? param = null)
        {
            return this.Query(connection => connection.Query<T>(query, param));
        }

        /// <summary>
        /// Executes on a new connection to the database.
        /// The connection will be safely closed after the operation.
        /// </summary>
        /// <param name="query">The SQL query to execute</param>
        /// <param name="param">The query parameters</param>
        public void Execute(string query, object? param = null)
        {
            this.Execute(connection => connection.Execute(query, param));
        }

        /// <summary>
        /// Executes a single row query
        /// The connection will be safely closed after the operation.
        /// </summary>
        /// <typeparam name="T">The type the query will return</typeparam>
        /// <param name="query">The SQL query to execute</param>
        /// <param name="param">The query parameters</param>
        /// <returns>The requested data, or null if not present.</returns>
        public T QueryFirstOrDefault<T>(string query, object? param = null)
        {
            return this.Query(connection => connection.QueryFirstOrDefault<T>(query, param));
        }

        /// <summary>
        /// Creates a table in the database
        /// </summary>
        /// <param name="tableName">The name of the table to create</param>
        /// <param name="columns">The names of the columns to create</param>
        public void CreateTable(string tableName, params string[] columns)
        {
            this.Execute($@"CREATE TABLE IF NOT EXISTS {tableName}({string.Join(",", columns)})");
        }

        /// <summary>
        /// Saves the in-memory databse to a real database
        /// </summary>
        /// <param name="database">The real database to back up to</param>
        public void SaveTo(SqliteDatabase database)
        {
            using (var conn = database.GetConnection())
            {
                conn.Open();
                this.dbConnection.BackupDatabase((SqliteConnection)conn);
                conn.Close();
            }
        }

        /// <summary>
        /// Loads from a disk database to this in-memory database
        /// </summary>
        /// <param name="database">The database to load from</param>
        public void LoadFrom(SqliteDatabase database)
        {
            using (var conn = database.GetConnection())
            {
                conn.Open();
                ((SqliteConnection)conn).BackupDatabase(this.dbConnection);
                conn.Close();
            }
        }
    }
}

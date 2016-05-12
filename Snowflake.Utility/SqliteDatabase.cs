using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using Dapper;

namespace Snowflake.Utility
{
    public class SqliteDatabase
    {
        public string FileName { get; }
        private readonly string dbConnectionString;

        public SqliteDatabase(string fileName)
        {
            this.FileName = fileName;

            if (!File.Exists(this.FileName))
            {
                SQLiteConnection.CreateFile(this.FileName);
            }
            this.dbConnectionString = $"Data Source={this.FileName};Version=3;PRAGMA journal_mode=WAL;";
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(this.dbConnectionString);
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
            T record;
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                record = queryFunction(dbConnection);
                if (dbConnection.State != ConnectionState.Closed) dbConnection.Close();
            }
            return record;
        }

        /// <summary>
        /// Executes on a new connection to the database.
        /// The connection will be safely closed after the operation.
        /// </summary>
        /// <param name="queryFunction">A function to query the database using the opened connection</param>
        public void Execute(Action<DbConnection> queryFunction)
        {
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                queryFunction(dbConnection);
                if (dbConnection.State != ConnectionState.Closed) dbConnection.Close();
            }
        }

        /// <summary>
        /// Queries on a new connection to the database.
        /// The connection will be safely closed after the operation.
        /// </summary>
        /// <typeparam name="T">The type the query will return</typeparam>
        /// <param name="query">The SQL query to execute</param>
        /// <param name="param">The query parameters</param>
        /// <returns>The requested data.</returns>
        public IEnumerable<T> Query<T>(string query, object param = null)
        {
            return this.Query(dbConnection => dbConnection.Query<T>(query, param));
        }

        /// <summary>
        /// Executes on a new connection to the database.
        /// The connection will be safely closed after the operation.
        /// </summary>
        /// <param name="query">The SQL query to execute</param>
        /// <param name="param">The query parameters</param>
        public void Execute(string query, object param = null)
        {
            this.Execute(dbConnection => dbConnection.Execute(query, param));
        }

        /// <summary>
        /// Executes a single row query
        /// The connection will be safely closed after the operation.
        /// </summary>
        /// <typeparam name="T">The type the query will return</typeparam>
        /// <param name="query">The SQL query to execute</param>
        /// <param name="param">The query parameters</param>
        /// <returns>The requested data, or null if not present.</returns>
        public T QueryFirstOrDefault<T>(string query, object param = null)
        {
            return this.Query(dbConnection => dbConnection.QueryFirstOrDefault<T>(query, param));
        }

        /// <summary>
        /// Creates a table in the database
        /// </summary>
        /// <param name="tableName">The name of the table to create</param>
        /// <param name="columns">The names of the columns to create</param>
        public void CreateTable(string tableName, params string[] columns)
        {

            this.Execute($@"CREATE TABLE IF NOT EXISTS {tableName}({String.Join(",", columns)})");
        }
    }
}

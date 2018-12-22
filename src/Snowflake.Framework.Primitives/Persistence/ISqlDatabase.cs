using System;
using System.Collections.Generic;
using System.Data;

namespace Snowflake.Persistence
{
    /// <summary>
    /// Represents a generic SQL database.
    /// Usually SQLite backed.
    /// </summary>
    public interface ISqlDatabase
    {
        /// <summary>
        /// Gets the file or connection name.
        /// </summary>
        string DatabaseName { get; }

        /// <summary>
        /// Creates a table with the given name and column rows
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="columns">The column directives.</param>
        void CreateTable(string tableName, params string[] columns);

        /// <summary>
        /// Executes an atomic query function on the database.
        /// </summary>
        /// <param name="queryFunction">The query function on the database</param>
        void Execute(Action<IDbConnection> queryFunction);

        /// <summary>
        /// Executes a parameterized string query given an object representing paramaters.
        /// </summary>
        /// <param name="query">The SQL string query</param>
        /// <param name="param">The object containing the parameters</param>
        void Execute(string query, object? param = null);

        /// <summary>
        /// Executes a typed query given a query function that returns the object.
        /// </summary>
        /// <typeparam name="T">The return type of the object</typeparam>
        /// <param name="queryFunction">The query function that returns the object</param>
        /// <returns></returns>
        T Query<T>(Func<IDbConnection, T> queryFunction);

        /// <summary>
        /// Executes a string query that returns an enumerable of matching objects.
        /// </summary>
        /// <typeparam name="T">The return type of the objects</typeparam>
        /// <param name="query">The parameterized string query.</param>
        /// <param name="param">The object containing the parameters</param>
        /// <returns>An enumerable or matching objects</returns>
        IEnumerable<T> Query<T>(string query, object? param = null);

        /// <summary>
        /// Returns the first object given the string query, or null if no matching objects found.
        /// </summary>
        /// <typeparam name="T">The return type of the objects</typeparam>
        /// <param name="query">The parameterized string query.</param>
        /// <param name="param">The object containing the parameters</param>
        /// <returns>The first object given the string query, or null if no matching objects found.</returns>
        T QueryFirstOrDefault<T>(string query, object? param = null);

        /// <summary>
        /// Gets a connection to the database
        /// </summary>
        /// <returns></returns>
        IDbConnection GetConnection();
    }
}
using System;
using System.Collections.Generic;
using System.Data;

namespace Snowflake.Persistence
{
    public interface ISqlDatabase
    {
        string FileName { get; }

        void CreateTable(string tableName, params string[] columns);
        void Execute(Action<IDbConnection> queryFunction);
        void Execute(string query, object param = null);
        IDbConnection GetConnection();
        T Query<T>(Func<IDbConnection, T> queryFunction);
        IEnumerable<T> Query<T>(string query, object param = null);
        T QueryFirstOrDefault<T>(string query, object param = null);
    }
}
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using Dapper;

namespace Snowflake.Utility
{
    public abstract class SqliteDatabase 
    {
        public string FileName { get; }
        private readonly string dbConnectionString;

        protected SqliteDatabase(string fileName)
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

        public T Query<T>(Func<DbConnection, T> queryFunction)
        {
            T record;
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                record = queryFunction(dbConnection);
                dbConnection.Close();
            }
            return record;
        }

        public void Execute(Action<DbConnection> queryFunction)
        {
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                queryFunction(dbConnection);
                dbConnection.Close();
            }
        }

        public IEnumerable<T> QuerySimple<T>(string query, object param = null)
        {
            return this.Query(dbConnection => dbConnection.Query<T>(query, param));
        }

        public void ExecuteSimple(string query, object param = null)
        {
            this.Execute(dbConnection => dbConnection.Execute(query, param));
        }

        public T QuerySingleSimple<T>(string query, object param = null)
        {
            return this.Query(dbConnection => dbConnection.QueryFirst<T>(query, param));
        }

  
    }
}

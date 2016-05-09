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

        public IEnumerable<T> QuerySimple<T>(string query, object param = null)
        {
            IEnumerable<T> records;
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                records = dbConnection.Query<T>(query, param);
                dbConnection.Close();
            }
            return records;
        }

        public void ExecuteSimple(string query, object param = null)
        {
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                dbConnection.Execute(query, param);
                dbConnection.Close();
            }
        }

        public T QuerySingleSimple<T>(string query, object param = null)
        {
            T record;
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                record = dbConnection.QueryFirst<T>(query);
                dbConnection.Close();
            }
            return record;
        }
    }
}

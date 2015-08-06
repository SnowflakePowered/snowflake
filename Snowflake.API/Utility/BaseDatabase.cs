using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;

namespace Snowflake.Utility
{
    public abstract class BaseDatabase
    {
        public string FileName { get; private set; }
        private readonly string DBConnectionString;
        public BaseDatabase(string fileName)
        {
            this.FileName = fileName;

            if (!File.Exists(this.FileName))
            {
                SQLiteConnection.CreateFile(this.FileName);
            }
            this.DBConnectionString = $"Data Source={this.FileName};Version=3;PRAGMA journal_mode=WAL;";
        }
        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(this.DBConnectionString);
        }
    }
}

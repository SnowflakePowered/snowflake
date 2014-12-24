using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;

namespace Snowflake.Database
{
    public abstract class BaseDatabase
    {
        public string FileName { get; private set; }
        protected SQLiteConnection DBConnection {get; set;}
        public BaseDatabase(string fileName)
        {
            this.FileName = fileName;

            if (!File.Exists(this.FileName))
            {
                SQLiteConnection.CreateFile(this.FileName);
            }
            this.DBConnection = new SQLiteConnection("Data Source=" + this.FileName + ";Version=3;");
        }
    }
}

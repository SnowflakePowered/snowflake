using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace Snowflake.API.Database
{
    public class GameDatabase
    {
        public string FileName{get; private set;}
        private SQLiteConnection DBConnection {get; set;}

        public GameDatabase(string fileName){
            this.FileName = FileName;

            if (!File.Exists(this.FileName))
            {
                this.CreateDatabase();
            }
            this.DBConnection = new SQLiteConnection("Data Source=" + this.FileName + ";Version=3;");
        }

        private void CreateDatabase()
        {
            SQLiteConnection.CreateFile(this.FileName);
            var dbConnection = new SQLiteConnection("Data Source="+this.FileName+";Version=3;");
            dbConnection.Open();
            SQLiteCommand sqlCommand = new SQLiteCommand(@"CREATE TABLE IF NOT EXISTS games(
                                                                platform_id TEXT,
                                                                uuid TEXT,
                                                                filename TEXT,
                                                                name TEXT,
                                                                images TEXT,
                                                                metadata TEXT,
                                                                settings TEXT
                                                                )", dbConnection);
            sqlCommand.ExecuteNonQuery();
            dbConnection.Close();
        }
    }
}

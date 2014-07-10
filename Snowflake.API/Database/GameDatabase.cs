using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using Snowflake.API.Information.Platform;
using Snowflake.API.Information.Game;

namespace Snowflake.API.Database
{
    public class GameDatabase
    {
        public string FileName{get; private set;}
        private SQLiteConnection DBConnection {get; set;}
        public GameDatabase(string fileName){
            this.FileName = fileName;

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

        public void AddGame(Game game){
            this.DBConnection.Open();
            using (SQLiteCommand sqlCommand = new SQLiteCommand(String.Format(@"INSERT INTO games VALUES(
                                          ""{0}"",
                                          ""{1}"",
                                          ""{2}"",
                                          ""{3}"",
                                          ""{4}"",
                                          ""{5}"",
                                          ""{6}""",
                                          game.PlatformId,
                                          game.UUID,
                                          game.FileName,
                                          game.Name,
                                          JsonConvert.SerializeObject(game.Images),
                                          JsonConvert.SerializeObject(game.Metadata),
                                          JsonConvert.SerializeObject(game.Settings)),
                                          this.DBConnection))
            {
                sqlCommand.ExecuteNonQuery();
            }
            this.DBConnection.Close();
        }

        public Game GetGameByUUID(string uuid)
        {
            this.DBConnection.Open();
            string query = @"SELECT * FROM `games` WHERE `uuid` == """ + uuid + @"""";
            using (SQLiteCommand sqlCommand = new SQLiteCommand(query,this.DBConnection)){
                using (var reader = sqlCommand.ExecuteReader(CommandBehavior.SingleResult))
                {
                    DataTable result = new DataTable();
                    result.Load(reader);
                    return GetGameFromDataRow(result.Rows[0]);
                }
            }
        }

        public IList<Game> GetGamesByPlatform(string platformId)
        {
            return GetGamesByRow("platform_id", platformId);
        }
        public IList<Game> GetGamesByName(string nameSearch)
        {
            return GetGamesByRow("name", nameSearch);
        }
        private IList<Game>GetGamesByRow(string rowName, string searchQuery){
            this.DBConnection.Open();
            string query = @"SELECT * FROM `games` WHERE `" + rowName + @"` == """ + searchQuery + @"""";
            using (SQLiteCommand sqlCommand = new SQLiteCommand(query, this.DBConnection))
            {
                using (var reader = sqlCommand.ExecuteReader())
                {
                    DataTable result = new DataTable();
                    result.Load(reader);
                    IList<Game> gamesResults = new List<Game>();
                    foreach (DataRow row in result.Rows)
                    {
                        gamesResults.Add(GetGameFromDataRow(row));
                    }
                    return gamesResults;
                }
            }
        }
        private Game GetGameFromDataRow(DataRow row)
        {
            var platformId = (string)row["platform_id"];
            var uuid = (string)row["uuid"];
            var fileName = (string)row["filename"];
            var name = (string)row["name"];
            var images = JsonConvert.DeserializeObject<GameImages>((string)row["images"]);
            var metadata = JsonConvert.DeserializeObject<IDictionary<string, string>>((string)row["metadata"]);
            var settings = JsonConvert.DeserializeObject<IDictionary<string, dynamic>>((string)row["settings"]);

            return new Game(platformId, name, images, metadata, uuid, fileName, settings);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using Snowflake.Information.Platform;
using Snowflake.Information.Game;

namespace Snowflake.Database
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
            using (SQLiteCommand sqlCommand = new SQLiteCommand(@"INSERT INTO games VALUES(
                                          @platform_id,
                                          @uuid,
                                          @filename,
                                          @name,
                                          @images,
                                          @metadata,
                                          @settings)", this.DBConnection))
            {
                sqlCommand.Parameters.AddWithValue("@platform_id", game.PlatformId);
                sqlCommand.Parameters.AddWithValue("@uuid", game.UUID);
                sqlCommand.Parameters.AddWithValue("@filename", game.FileName);
                sqlCommand.Parameters.AddWithValue("@name",  game.Name);
                sqlCommand.Parameters.AddWithValue("@images", JsonConvert.SerializeObject(game.Images));
                sqlCommand.Parameters.AddWithValue("@metadata",  JsonConvert.SerializeObject(game.Metadata));
                sqlCommand.Parameters.AddWithValue("@settings", JsonConvert.SerializeObject(game.Settings));
                sqlCommand.ExecuteNonQuery();
            }
            this.DBConnection.Close();
        }

        public Game GetGameByUUID(string uuid)
        {
            return GetGamesByColumn("uuid", uuid)[0];
        }

        public IList<Game> GetGamesByPlatform(string platformId)
        {
            return GetGamesByColumn("platform_id", platformId);
        }
        public IList<Game> GetGamesByName(string nameSearch)
        {
            return GetGamesByColumn("name", nameSearch);
        }
        private IList<Game>GetGamesByColumn(string colName, string searchQuery){
            this.DBConnection.Open();
            using (SQLiteCommand sqlCommand = new SQLiteCommand(@"SELECT * FROM `games` WHERE `%colName` == @searchQuery"
                , this.DBConnection))
            {
                sqlCommand.CommandText = sqlCommand.CommandText.Replace("%colName", colName); //Easier to read than string replacement.
                sqlCommand.Parameters.AddWithValue("@searchQuery", searchQuery);
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

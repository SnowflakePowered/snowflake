using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using Snowflake.Platform;
using Snowflake.Game;
using Snowflake.Information.MediaStore;
using Snowflake.Utility;

namespace Snowflake.Game
{
    public class GameDatabase : BaseDatabase, IGameDatabase
    {
        public GameDatabase(string fileName)
            : base(fileName)
        {
            this.CreateDatabase();
        }

        private void CreateDatabase()
        {
            this.DBConnection.Open();
            var sqlCommand = new SQLiteCommand(@"CREATE TABLE IF NOT EXISTS games(
                                                                platform_id TEXT,
                                                                uuid TEXT PRIMARY KEY,
                                                                filename TEXT,
                                                                name TEXT,
                                                                mediastorekey TEXT,
                                                                metadata TEXT,
                                                                crc32 TEXT
                                                                )", this.DBConnection);
            sqlCommand.ExecuteNonQuery();
            this.DBConnection.Close();
        }

        public void AddGame(IGameInfo game)
        {
            this.DBConnection.Open();
            using (var sqlCommand = new SQLiteCommand(@"INSERT OR REPLACE INTO games VALUES(
                                          @platform_id,
                                          @uuid,
                                          @filename,
                                          @name,
                                          @mediastorekey,
                                          @metadata,
                                          @crc32)", this.DBConnection))
            {
                sqlCommand.Parameters.AddWithValue("@platform_id", game.PlatformId);
                sqlCommand.Parameters.AddWithValue("@uuid", game.UUID);
                sqlCommand.Parameters.AddWithValue("@filename", game.FileName);
                sqlCommand.Parameters.AddWithValue("@name", game.Name);
                sqlCommand.Parameters.AddWithValue("@mediastorekey", game.MediaStore.MediaStoreKey);
                sqlCommand.Parameters.AddWithValue("@metadata", JsonConvert.SerializeObject(game.Metadata));
                sqlCommand.Parameters.AddWithValue("@crc32", game.CRC32);
                sqlCommand.ExecuteNonQuery();
            }
            this.DBConnection.Close();
        }

        public IGameInfo GetGameByUUID(string uuid)
        {
            try
            {
                return GetGamesByColumn("uuid", uuid)[0];
            }
            catch
            {
                return null;
            }
        }

        public IList<IGameInfo> GetGamesByPlatform(string platformId)
        {
            return GetGamesByColumn("platform_id", platformId);
        }
        public IList<IGameInfo> GetGamesByName(string nameSearch)
        {
            return GetGamesByColumn("name", nameSearch);
        }
        private IList<IGameInfo> GetGamesByColumn(string colName, string searchQuery)
        {
            this.DBConnection.Open();
            using (var sqlCommand = new SQLiteCommand(@"SELECT * FROM `games` WHERE `%colName` == @searchQuery"
                , this.DBConnection))
            {
                sqlCommand.CommandText = sqlCommand.CommandText.Replace("%colName", colName); //Easier to read than string replacement.
                sqlCommand.Parameters.AddWithValue("@searchQuery", searchQuery);
                using (var reader = sqlCommand.ExecuteReader())
                {
                    var result = new DataTable();
                    result.Load(reader);
                    var gamesResults = (from DataRow row in result.Rows select GetGameFromDataRow(row)).ToList();
                    this.DBConnection.Close();
                    return gamesResults;
                }
            }
        }
        public IList<IGameInfo> GetAllGames()
        {
            this.DBConnection.Open();
            using (var sqlCommand = new SQLiteCommand(@"SELECT * FROM `games`"
                , this.DBConnection))
            {
                using (var reader = sqlCommand.ExecuteReader())
                {
                    var result = new DataTable();
                    result.Load(reader);
                    var gamesResults = (from DataRow row in result.Rows select this.GetGameFromDataRow(row)).ToList();
                    this.DBConnection.Close();
                    return gamesResults;
                }
            }
        }
        private IGameInfo GetGameFromDataRow(DataRow row)
        {
            var platformId = row.Field<string>("platform_id");
            var uuid = row.Field<string>("uuid");
            var fileName = row.Field<string>("filename");
            var name = row.Field<string>("name");
            var mediaStore = new FileMediaStore(row.Field<string>("mediastorekey"));
            var metadata = JsonConvert.DeserializeObject<IDictionary<string, string>>(row.Field<string>("metadata"));
            var crc32 = row.Field<string>("crc32");

            return new GameInfo(platformId, name, mediaStore, metadata, uuid, fileName, crc32);
        }

    }
}

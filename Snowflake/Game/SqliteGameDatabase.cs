using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using Newtonsoft.Json;
using Snowflake.Game;
using Snowflake.Utility;

namespace Snowflake.Information.Database
{
    /// <summary>
    /// Represents the database logic behind a game library
    /// </summary>
    internal class SqliteGameDatabase : SqliteDatabase, IGameLibrary
    {
        public SqliteGameDatabase(string fileName) : base(fileName)
        {
            this.CreateDatabase();
            SqlMapper.AddTypeHandler(new DictionaryStringStringTypeHandler());
        }

        private void CreateDatabase()
        {
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                dbConnection.Execute(@"CREATE TABLE IF NOT EXISTS games(
                                                                platform_id TEXT,
                                                                uuid TEXT PRIMARY KEY,
                                                                filename TEXT,
                                                                title TEXT,
                                                                metadata TEXT,
                                                                crc32 TEXT
                                                                )", dbConnection);
                dbConnection.Close();
            }
        }

        void IGameLibrary.AddGame(IGameInfo game)
        {
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                dbConnection.Execute(@"INSERT OR REPLACE INTO games(platform_id, uuid, filename, title, metadata, crc32) 
                                        VALUES (
                                          @platform_id,
                                          @uuid,
                                          @filename,
                                          @title,
                                          @metadata,
                                          @crc32)",
                    new
                    {
                        platform_id = game.PlatformID,
                        uuid = game.UUID,
                        filename = game.FileName,
                        title = game.Title,
                        metadata = JsonConvert.SerializeObject(game.Metadata),
                        crc32 = game.CRC32
                    });
                dbConnection.Close();
            }
        }

        IEnumerable<IGameInfo> IGameLibrary.GetAllGames()
        {
            IEnumerable<dynamic> gamesList;
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                gamesList = dbConnection.Query("SELECT * FROM games");
                dbConnection.Close();
            }
            return gamesList.Select(game => 
            new GameInfo(game.uuid,
            game.platform_id, 
            game.filename,
            game.name,
            game.crc32, 
            JsonConvert.DeserializeObject<IDictionary<string, string>>(game.metadata)));
        }

        IGameInfo IGameLibrary.GetGameByUUID(string uuid)
        {
            return this.GetGamesByColumn("uuid", uuid).FirstOrDefault();
        }

        IEnumerable<IGameInfo> IGameLibrary.GetGamesByName(string nameSearch)
        {
            return this.GetGamesByColumn("title", nameSearch);
        }

        IEnumerable<IGameInfo> IGameLibrary.GetGamesByPlatform(string platformId)
        {
            return this.GetGamesByColumn("platform_id", platformId);
        }

        void IGameLibrary.RemoveGame(IGameInfo game)
        {
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                dbConnection.Execute("DELETE FROM games WHERE uuid = @uuid", new { uuid = game.UUID });
                dbConnection.Close();
            }
        }

        private IEnumerable<IGameInfo> GetGamesByColumn(string colName, string searchQuery)
        {
            IEnumerable<dynamic> gamesList;
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                gamesList = dbConnection.Query($@"SELECT * FROM games WHERE {colName} = @searchQuery",
                    new { searchQuery });
                dbConnection.Close();
            }
            return gamesList.Select(game =>
                new GameInfo(game.uuid,
                game.platform_id,
                game.filename,
                game.title,
                game.crc32,
                JsonConvert.DeserializeObject<IDictionary<string, string>>(game.metadata)));

        }
    }
}

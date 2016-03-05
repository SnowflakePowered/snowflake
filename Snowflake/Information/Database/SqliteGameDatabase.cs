using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using DapperExtensions;
using Snowflake.Game;
using Snowflake.Utility;

namespace Snowflake.Information.Database
{
    /// <summary>
    /// Represents the database logic behind a game library
    /// </summary>
    internal class SqliteGameDatabase : DapperDatabase, IGameLibrary
    {
        public SqliteGameDatabase(string fileName) : base(fileName, typeof(SqliteGameDatabaseMapper))
        {
            this.CreateDatabase();
            SqlMapper.AddTypeHandler(new DictionaryStringStringTypeHandler());
        }

        private void CreateDatabase()
        {
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                var sqlCommand = new SQLiteCommand(@"CREATE TABLE IF NOT EXISTS games(
                                                                platform_id TEXT,
                                                                uuid TEXT PRIMARY KEY,
                                                                filename TEXT,
                                                                name TEXT,
                                                                metadata TEXT,
                                                                crc32 TEXT
                                                                )", dbConnection);
                sqlCommand.ExecuteNonQuery();
                dbConnection.Close();
            }
        }

        void IGameLibrary.AddGame(IGameInfo game)
        {
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                dbConnection.Insert(game);
                dbConnection.Close();
            }
        }

        IEnumerable<IGameInfo> IGameLibrary.GetAllGames()
        {
            IEnumerable<IGameInfo> gamesList;
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                gamesList = dbConnection.GetList<GameInfo>();
                dbConnection.Close();
            }
            return gamesList;
        }

        IGameInfo IGameLibrary.GetGameByUUID(string uuid)
        {
            IGameInfo uuidGame;
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                uuidGame = dbConnection.Get<GameInfo>(uuid);
                dbConnection.Close();
            }
            return uuidGame;
        }

        IEnumerable<IGameInfo> IGameLibrary.GetGamesByName(string nameSearch)
        {
            IEnumerable<IGameInfo> gamesList;
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                var predicate = Predicates.Field<GameInfo>(game => game.Name, Operator.Eq, nameSearch);
                gamesList = dbConnection.GetList<GameInfo>(predicate);
                dbConnection.Close();
            }
            return gamesList;
        }

        IEnumerable<IGameInfo> IGameLibrary.GetGamesByPlatform(string platformId)
        {
            IEnumerable<IGameInfo> gamesList;
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                var predicate = Predicates.Field<GameInfo>(game => game.PlatformID, Operator.Eq, platformId);
                gamesList = dbConnection.GetList<GameInfo>(predicate);
                dbConnection.Close();
            }
            return gamesList;
        }

        void IGameLibrary.RemoveGame(IGameInfo game)
        {
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                dbConnection.Delete(game);
                dbConnection.Close();
            }
        }
    }
}

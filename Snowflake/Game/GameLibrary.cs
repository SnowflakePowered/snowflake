using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Newtonsoft.Json;
using Snowflake.Records.Game;
using Snowflake.Utility;

namespace Snowflake.Game
{
    [Obsolete("Will be replaced with Snowflake.Records.GameLibrary")]
    public class GameLibrary : IGameLibrary
    {
        private readonly Records.Game.IGameLibrary backingGameDatabase;
        public GameLibrary(string fileName)
        {
            this.backingGameDatabase = new SqliteGameLibrary(new SqliteDatabase(fileName));
        }

        public void AddGame(IGameInfo game)
        {
            this.backingGameDatabase.Set(game);
        }

        public void RemoveGame(IGameInfo game)
        {
            this.backingGameDatabase.Remove(game);
        }

        public IGameInfo GetGameByUUID(string uuid)
        {
            var gameInfo = this.backingGameDatabase.GetByMetadata("obsolete_uuid", uuid).FirstOrDefault();
            return gameInfo == null ? null : new GameInfo(gameInfo);
        }

        public IEnumerable<IGameInfo> GetGamesByPlatform(string platformId)
        {
            return this.backingGameDatabase.GetGamesByPlatform(platformId).Select(g => new GameInfo(g));
        }

        public IEnumerable<IGameInfo> GetGamesByName(string nameSearch)
        {
            return this.backingGameDatabase.GetGamesByTitle(nameSearch).Select(g => new GameInfo(g));
        }
        
        public IEnumerable<IGameInfo> GetAllGames()
        {
            return this.backingGameDatabase.GetAllRecords().Select(g => new GameInfo(g));
        }
    }
}

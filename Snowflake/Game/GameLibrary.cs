using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Newtonsoft.Json;
using Snowflake.Game.Database;
using Snowflake.Utility;

namespace Snowflake.Game
{
    public class GameLibrary : IGameLibrary
    {
        private readonly IGameLibrary backingGameDatabase;
        public GameLibrary(string fileName)
        {
            this.backingGameDatabase = new GameDatabase(fileName);
        }

        public void AddGame(IGameInfo game)
        {
            this.backingGameDatabase.AddGame(game);
        }

        public void RemoveGame(IGameInfo game)
        {
            this.backingGameDatabase.RemoveGame(game);
        }

        public IGameInfo GetGameByUUID(string uuid)
        {
            return this.backingGameDatabase.GetGameByUUID(uuid);
        }

        public IEnumerable<IGameInfo> GetGamesByPlatform(string platformId)
        {
            return this.backingGameDatabase.GetGamesByPlatform(platformId);
        }

        public IEnumerable<IGameInfo> GetGamesByName(string nameSearch)
        {
            return this.backingGameDatabase.GetGamesByName(nameSearch);
        }
        
        public IEnumerable<IGameInfo> GetAllGames()
        {
            return this.backingGameDatabase.GetAllGames();
        }
    }
}

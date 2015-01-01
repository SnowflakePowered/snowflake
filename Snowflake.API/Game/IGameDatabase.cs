using System;
using System.Collections.Generic;
namespace Snowflake.Game
{
    public interface IGameDatabase
    {
        void AddGame(IGameInfo game);
        IList<IGameInfo> GetAllGames();
        IGameInfo GetGameByUUID(string uuid);
        IList<IGameInfo> GetGamesByName(string nameSearch);
        IList<IGameInfo> GetGamesByPlatform(string platformId);
    }
}

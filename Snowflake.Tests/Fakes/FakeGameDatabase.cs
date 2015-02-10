using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Game;

namespace Snowflake.Tests.Fakes 
{
    internal class FakeGameDatabase : IGameDatabase
    {
        public void AddGame(IGameInfo game)
        {
            throw new NotImplementedException();
        }

        public IList<IGameInfo> GetAllGames()
        {
            throw new NotImplementedException();
        }

        public IGameInfo GetGameByUUID(string uuid)
        {
            throw new NotImplementedException();
        }

        public IList<IGameInfo> GetGamesByName(string nameSearch)
        {
            throw new NotImplementedException();
        }

        public IList<IGameInfo> GetGamesByPlatform(string platformId)
        {
            throw new NotImplementedException();
        }
    }
}

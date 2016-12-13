using Snowflake.Records.Game;
using Snowflake.Services;
using Snowflake.Support.Remoting.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Support.Remoting.Framework.Exceptions;

namespace Snowflake.Support.Remoting.Resources
{
    public class Games
    {

        private IGameLibrary Library { get; }
        public Games(ICoreService core)
        {
            this.Library = core.Get<IGameLibrary>();
        }

        
        [Endpoint(RemotingVerbs.Read, "~:games")]
        public IEnumerable<IGameRecord> ListGames()
        {
            return this.Library.GetAllRecords();
        }

        [Endpoint(RemotingVerbs.Read, "~:games:{guid}")]
        public IGameRecord GetGame(Guid game) 
        {
            try 
            {
            return this.Library.Get(game);
            }
            catch(Exception)
            {
                throw new GameNotFoundException(game);
            } 
        }
    }
}

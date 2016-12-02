using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Services;
using Snowflake.Records.Game;
using System.Linq;
using Snowflake.Support.Remoting.Framework;

namespace Snowflake.Support.Remoting.Endpoints
{
    public class Games : Endpoint
    {
        private IGameLibrary Library { get; }
        public Games(ICoreService coreService) : base(coreService)
        {
            this.Library = coreService.Get<IGameLibrary>();
        }

        public IList<IGameRecord> ListGames()
        {
            return this.Library?.GetAllRecords().ToList();
        }

        [Endpoint("/games/{uuid}/")]
        public IGameRecord GetGame(Guid uuid)
        {
            return this.Library?.Get(uuid);
        }
    }
}

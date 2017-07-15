using Snowflake.Records.Game;
using Snowflake.Remoting.Resources;
using Snowflake.Remoting.Resources.Attributes;
using Snowflake.Services;
using Snowflake.Support.Remoting.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Resources.Games
{
    [Resource("games", ":guid")]
    [Parameter(typeof(Guid), "guid")]
    public class GameRoot : Resource
    {
        private IGameLibrary Library { get; }
        public GameRoot(IGameLibrary library)
        {
            this.Library = library;
        }

        [Endpoint(EndpointVerb.Read)]
        public IGameRecord GetGame(Guid guid)
        {
            var game = this.Library.Get(guid);
            if (game == null) throw new RecordNotFoundException(guid);
            return game;
        }

        [Endpoint(EndpointVerb.Delete)]
        public IEnumerable<IGameRecord> DeleteGame(Guid guid)
        {
            this.Library.Remove(guid);
            return this.Library.GetAllRecords();
        }
    }
}

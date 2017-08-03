using Snowflake.Records.Game;
using Snowflake.Services;
using Snowflake.Support.Remoting.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Support.Remoting.Framework.Exceptions;
using Snowflake.Records.Metadata;
using Snowflake.Records.File;
using Snowflake.Scraper;
using Snowflake.Remoting.Resources.Attributes;
using Snowflake.Remoting.Resources;
using Snowflake.Platform;

namespace Snowflake.Support.Remoting.Resources
{
    [Resource("games")]
    public class GameLibraryRoot : Resource
    {
        private IGameLibrary Library { get; }

        public GameLibraryRoot(IGameLibrary library)
        {
            this.Library = library;
        }

        [Endpoint(EndpointVerb.Read)]
        public IEnumerable<IGameRecord> ListGames()
        {
            return this.Library.GetAllRecords();
        }

        [Endpoint(EndpointVerb.Create)]
        [Parameter(typeof(IPlatformInfo), "platform")]
        [Parameter(typeof(string), "title")]
        public IGameRecord CreateGame(string title, IPlatformInfo platform)
        {
            var record = new GameRecord(platform, title);
            this.Library.Set(record);
            return this.Library.Get(record.Guid);
        }
    }
}

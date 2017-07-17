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

namespace Snowflake.Support.Remoting.Resources
{
    [Resource("games")]
    public class GameLibraryRoot : Resource
    {
        private IGameLibrary Library { get; }
        private IStoneProvider Stone { get; }

        public GameLibraryRoot(IStoneProvider stone, IGameLibrary library)
        {
            this.Library = library;
            this.Stone = stone;
        }

        [Endpoint(EndpointVerb.Read)]
        public IEnumerable<IGameRecord> ListGames()
        {
            return this.Library.GetAllRecords();
        }

        [Endpoint(EndpointVerb.Create)]
        [Parameter(typeof(string), "platform")]
        [Parameter(typeof(string), "title")]
        public IGameRecord CreateGame(string title, string platform)
        {
            try
            {
                var platformInfo = this.Stone.Platforms[platform];
                var record = new GameRecord(platformInfo, title);
                this.Library.Set(record);
                return this.Library.Get(record.Guid);
            }
            catch (KeyNotFoundException)
            {
                throw new UnknownPlatformException(platform);
            }
        }
    }
}

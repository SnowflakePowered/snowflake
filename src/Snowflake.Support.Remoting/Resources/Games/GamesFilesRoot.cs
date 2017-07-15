using Snowflake.Records.Game;
using Snowflake.Remoting.Resources;
using Snowflake.Remoting.Resources.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Resources.Games
{
    [Resource("game", ":gameGuid", "files", ":fileGuid")]
    [Parameter(typeof(Guid), "gameGuid")]
    [Parameter(typeof(Guid), "fileGuid")]
    public class GamesFilesRoot : Resource
    {
        private IGameLibrary Library { get; }
        public GamesFilesRoot(IGameLibrary library)
        {
            this.Library = library;
        }

        [Endpoint(EndpointVerb.Delete)]
        public IGameRecord DeleteFile(Guid gameGuid, Guid fileGuid)
        {
            this.Library.FileLibrary.Remove(fileGuid);
            return this.Library.Get(gameGuid);
        }
    }
}

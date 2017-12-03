using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Remoting.Resources;
using Snowflake.Remoting.Resources.Attributes;
using Snowflake.Support.Remoting.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Resources.Games
{
    //"~:games:{guid}:files",
    [Resource("game", ":gameGuid", "files")]
    [Parameter(typeof(Guid), "guid")]
    public class GameFilesLibraryRoot : Resource
    {
        private IGameLibrary Library { get; }

        public GameFilesLibraryRoot(IGameLibrary library)
        {
            this.Library = library;
        }

        [Endpoint(EndpointVerb.Create)]
        [Parameter(typeof(string), "path")]
        [Parameter(typeof(string), "mimetype")]
        public IGameRecord CreateFile(Guid gameGuid, string path, string mimetype)
        {
            if (path == null || mimetype == null) throw new InvalidFileException();
            var file = new FileRecord(path, mimetype);
            this.Library.FileLibrary.Set(file);
            return this.Library.Get(gameGuid);
        }
    }
}

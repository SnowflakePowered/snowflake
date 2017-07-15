using Snowflake.Records.Game;
using Snowflake.Records.Metadata;
using Snowflake.Remoting.Resources;
using Snowflake.Remoting.Resources.Attributes;
using Snowflake.Support.Remoting.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Resources.Games
{
    [Resource("games", ":gameGuid", "files", ":fileGuid", "metadata", ":metadata_key")]
    [Parameter(typeof(Guid), ":gameGuid")]
    [Parameter(typeof(Guid), ":fileGuid")]
    [Parameter(typeof(string), "metadata_key")]
    public class GameFilesMetadataRoot
    {
        private List<string> ForbiddenDeleteKeys = new List<string> { "game_title", "game_platform", "file_linkedrecord" };
        private IGameLibrary Library { get; }
        public GameFilesMetadataRoot(IGameLibrary library)
        {
            this.Library = library;
        }

        [Endpoint(EndpointVerb.Delete)]
        public IGameRecord DeleteFileMetadata(Guid gameGuid, Guid fileGuid, string metadataKey)
        {
            if (this.ForbiddenDeleteKeys.Contains(metadataKey)) throw new ProtectedMetadataException(metadataKey);
            try
            {
                this.Library.MetadataLibrary.Remove((this.Library.FileLibrary.Get(fileGuid).Metadata as IDictionary<string, IRecordMetadata>)[metadataKey]);

            }
            catch (KeyNotFoundException)
            {
                throw new MetadataNotFoundException(fileGuid, metadataKey);
            }
            return this.Library.Get(gameGuid);
        }

        [Endpoint(EndpointVerb.Update)]
        [Parameter(typeof(string), "value")]
        public IGameRecord SetFileMetadata(Guid gameGuid, Guid fileGuid, string metadataKey, string value)
        {
            this.Library.MetadataLibrary.Set(new RecordMetadata(metadataKey, value, fileGuid));
            try
            {
                return this.Library.Get(gameGuid);
            }
            catch (Exception)
            {
                throw new RecordNotFoundException(gameGuid);
            }
        }
    }
}

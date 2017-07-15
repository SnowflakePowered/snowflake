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
    //:games:{guid}:metadata:{metadata_key}
    [Resource("games", ":guid", "metadata", ":metadata_key")]
    [Parameter(typeof(Guid), "guid")]
    [Parameter(typeof(string), "metadata_key")]
    public class GameMetadataRoot : Resource
    {
        private List<string> ForbiddenDeleteKeys = new List<string> { "game_title", "game_platform", "file_linkedrecord" };
        private IGameLibrary Library { get; }

        public GameMetadataRoot(IGameLibrary library)
        {
            this.Library = library;
        }

        [Endpoint(EndpointVerb.Update)]
        [Parameter(typeof(string), "value")]
        public IGameRecord SetMetadata(Guid game, string metadataKey, string value)
        {
            this.Library.MetadataLibrary.Set(new RecordMetadata(metadataKey, value, game));
            try
            {
                return this.Library.Get(game);
            }
            catch (Exception)
            {
                throw new RecordNotFoundException(game);
            }
        }

        [Endpoint(EndpointVerb.Delete)]
        public IGameRecord DeleteMetadata(Guid game, string metadataKey)
        {
            if (this.ForbiddenDeleteKeys.Contains(metadataKey)) throw new ProtectedMetadataException(metadataKey);
            try
            {
                this.Library.MetadataLibrary.Remove((this.Library.Get(game).Metadata as IDictionary<string, IRecordMetadata>)[metadataKey]);

            }
            catch (KeyNotFoundException)
            {
                throw new MetadataNotFoundException(game, metadataKey);
            }
            return this.Library.Get(game);
        }
    }
}

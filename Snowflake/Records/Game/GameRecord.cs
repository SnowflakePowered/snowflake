using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Platform;
using Snowflake.Records.File;
using Snowflake.Records.Metadata;

namespace Snowflake.Records.Game
{
    public class GameRecord : IGameRecord
    {
        public IDictionary<string, IRecordMetadata> Metadata { get; }
        public Guid Guid { get; }
        public string PlatformId => this.Metadata[GameMetadataKeys.Platform].Value;
        public string Title => this.Metadata[GameMetadataKeys.Title].Value;
        public IEnumerable<IFileRecord> Files { get; }

        public GameRecord(Guid guid, IDictionary<string, IRecordMetadata> metadata, IEnumerable<IFileRecord> files)
        {
            this.Guid = guid;
            this.Metadata = metadata;
            this.Files = files;
        }

        public GameRecord(IPlatformInfo platformInfo, string title, IEnumerable<IFileRecord> files)
        {
            this.Guid = Guid.NewGuid();
            this.Metadata = new Dictionary<string, IRecordMetadata>()
            {
                {
                    GameMetadataKeys.Platform,
                    new RecordMetadata(GameMetadataKeys.Platform, platformInfo.PlatformID, this.Guid)
                },
                {
                    GameMetadataKeys.Title,
                    new RecordMetadata(GameMetadataKeys.Title, title, this.Guid)
                }
            };
            this.Files = files;
        }
    }
}

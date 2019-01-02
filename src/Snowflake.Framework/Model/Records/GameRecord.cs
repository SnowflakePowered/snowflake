using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Model.Game;
using Snowflake.Model.Records.Game;
using Snowflake.Platform;

namespace Snowflake.Model.Records
{
    public class GameRecord : IGameRecord
    {
        internal GameRecord(PlatformId platform,
            Guid recordId,
            IMetadataCollection metadata)
        {
            this.PlatformId = platform;
            this.RecordId = recordId;
            this.Metadata = metadata;
        }

        public PlatformId PlatformId { get; }

        public string? Title
        {
            get => this.Metadata[GameMetadataKeys.Title];
            set => this.Metadata[GameMetadataKeys.Title] = value;
        }

        public IMetadataCollection Metadata { get; }

        public Guid RecordId { get; }
    }
}

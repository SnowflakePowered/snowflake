using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Model.Game;
using Snowflake.Model.Records.Game;

namespace Snowflake.Model.Records
{
    public class GameRecord : IGameRecord
    {
        internal GameRecord(PlatformId platform,
            Guid recordId,
            IMetadataCollection metadata
        )
        {
            this.PlatformID = platform;
            this.RecordID = recordId;
            this.Metadata = metadata;
        }

        public PlatformId PlatformID { get; }

        public string? Title
        {
            get => this.Metadata[GameMetadataKeys.Title];
            set => this.Metadata[GameMetadataKeys.Title] = value;
        }

        public IMetadataCollection Metadata { get; }
        public Guid RecordID { get; }
    }
}

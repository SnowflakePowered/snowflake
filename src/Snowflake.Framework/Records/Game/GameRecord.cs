﻿using System;
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
        /// <inheritdoc/>
        public IMetadataCollection Metadata { get; }

        /// <inheritdoc/>
        public Guid Guid { get; }

        /// <inheritdoc/>
        public string PlatformID => this.Metadata[GameMetadataKeys.Platform];

        /// <inheritdoc/>
        public string Title => this.Metadata[GameMetadataKeys.Title];

        /// <inheritdoc/>
        public IList<IFileRecord> Files { get; }

        internal GameRecord(Guid guid, IDictionary<string, IRecordMetadata> metadata, IList<IFileRecord> files)
        {
            this.Guid = guid;
            this.Metadata = new MetadataCollection(this.Guid, metadata);
            this.Files = files;
        }

        public GameRecord(IPlatformInfo platformInfo, string title)
        {
            this.Guid = Guid.NewGuid();
            this.Metadata = new MetadataCollection(this.Guid)
            {
                {
                    GameMetadataKeys.Platform,
                    new RecordMetadata(GameMetadataKeys.Platform, platformInfo.PlatformID, this.Guid)
                },
                {
                    GameMetadataKeys.Title,
                    new RecordMetadata(GameMetadataKeys.Title, title, this.Guid)
                },
            };
            this.Files = new List<IFileRecord>();
        }

        public GameRecord(IPlatformInfo platformInfo, string title, string filename, string mimetype)
            : this(platformInfo, title)
        {
            this.Files.Add(new FileRecord(filename, mimetype));
        }
    }
}

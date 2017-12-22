using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Records.Metadata;

namespace Snowflake.Records.File
{
    public class FileRecord : IFileRecord
    {
        /// <inheritdoc/>
        public IMetadataCollection Metadata { get; }

        /// <inheritdoc/>
        public Guid Guid { get; }

        /// <inheritdoc/>
        public string MimeType { get; }

        /// <inheritdoc/>
        public string FilePath { get; }

        internal FileRecord(Guid guid, IDictionary<string, IRecordMetadata> metadata, string filePath, string mimeType)
        {
            this.MimeType = mimeType;
            this.FilePath = filePath;
            this.Guid = guid;
            this.Metadata = new MetadataCollection(this.Guid, metadata);
        }

        public FileRecord(string filePath, string mimeType)
            : this(Guid.NewGuid(), new Dictionary<string, IRecordMetadata>(), filePath, mimeType)
        {
        }
    }
}

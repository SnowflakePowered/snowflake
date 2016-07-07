using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Records.Metadata;
using Snowflake.Utility;

namespace Snowflake.Records.File
{
    public class FileRecord : IFileRecord
    {
        public IMetadataCollection Metadata { get; }
        public Guid Guid { get; }
        public string MimeType { get; }
        public string FilePath { get; }
        public Guid Record => new Guid(this.Metadata["file_linkedrecord"]);

        internal FileRecord(Guid guid, IDictionary<string, IRecordMetadata> metadata, string filePath, string mimeType)
        {
            this.MimeType = mimeType;
            this.FilePath = filePath;
            this.Guid = guid;
            this.Metadata = new MetadataCollection(this.Guid, metadata);
        }

        internal FileRecord(IFileRecord record, IGameRecord game)
        {
            this.MimeType = record.MimeType;
            this.FilePath = record.FilePath;
            this.Guid = record.Guid;
            this.Metadata = record.Metadata;
            this.Metadata["file_linkedrecord"] = game.Guid.ToString();
        }


        public FileRecord(string filePath, string mimeType, Guid record):
            this(Guid.NewGuid(), new Dictionary<string, IRecordMetadata>(), filePath, mimeType)
        {
            this.Metadata["file_linkedrecord"] = record.ToString();
        }


        public FileRecord(string filePath, string mimeType, IGameRecord gameRecord) :
            this(filePath, mimeType, gameRecord.Guid)
        {
        }

        public FileRecord(string filePath, string mimeType)
            : this(Guid.NewGuid(), new Dictionary<string, IRecordMetadata>(), filePath, mimeType)
        {

        }
    }
}

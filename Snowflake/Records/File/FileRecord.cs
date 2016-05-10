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
        public IDictionary<string, IRecordMetadata> Metadata { get; }
        public Guid Guid { get; }
        public string MimeType { get; }
        public string FilePath { get; }
        public Guid GameRecord { get; }

        public FileRecord(Guid guid, Guid gameRecord, IDictionary<string, IRecordMetadata> metadata, string filePath, string mimeType)
        {
            this.Metadata = metadata;
            this.Guid = guid;
            this.GameRecord = gameRecord;
            this.MimeType = mimeType;
            this.FilePath = filePath;
        }

        public FileRecord(string filePath, string mimeType, Guid gameRecord):
            this(Guid.NewGuid(), gameRecord, new Dictionary<string, IRecordMetadata>(), filePath, mimeType)
        {
            
        }
    }
}

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
        public IDictionary<string, IRecordMetadata> Metadata { get; }
        public Guid Guid { get; }
        public string MimeType { get; }
        public string FilePath { get; }
        public Guid Record { get; }

        internal FileRecord(Guid record, IDictionary<string, IRecordMetadata> metadata, string filePath, string mimeType)
        {
            this.Metadata = metadata;
            this.Record = record;
            this.MimeType = mimeType;
            this.FilePath = filePath;
            this.Guid = GuidUtility.Create(this.Record, this.FilePath);

        }

        public FileRecord(string filePath, string mimeType, Guid record):
            this(record, new Dictionary<string, IRecordMetadata>(), filePath, mimeType)
        {
            
        }


        public FileRecord(string filePath, string mimeType, IGameRecord gameRecord) :
            this(gameRecord.Guid, new Dictionary<string, IRecordMetadata>(), filePath, mimeType)
        {

        }

        public bool Equals(IFileRecord file) => this.Guid == file.Guid;
        public override int GetHashCode() => this.Guid.GetHashCode();

        public override bool Equals(object file)
        {
            IFileRecord f = file as IFileRecord;

            if (f == null)
            {
                return false;
            }

            // Return true if the fields match:
            return f.Guid == this.Guid;
        }

        public static bool operator ==(FileRecord fileX, IFileRecord fileY) => fileX.Equals(fileY);
        public static bool operator !=(FileRecord fileX, IFileRecord fileY) => !(fileX == fileY);
    }
}

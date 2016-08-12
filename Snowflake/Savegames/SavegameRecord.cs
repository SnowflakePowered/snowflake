using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.File;
using Snowflake.Records.Metadata;

namespace Snowflake.Savegames
{
    public class SavegameRecord : IFileRecord
    {
        internal SavegameRecord(IFileRecord record)
        {
            this.Metadata = record.Metadata;
            this.Guid = record.Guid;
            this.MimeType = record.MimeType;
            this.FilePath = record.FilePath;
            this.Record = record.Record;
        }

        public IMetadataCollection Metadata { get; }
        public Guid Guid { get; }
        public string MimeType { get; }
        public string FilePath { get; }
        public Guid Record { get; }
    }
}

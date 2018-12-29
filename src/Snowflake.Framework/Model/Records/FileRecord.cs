using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Model.FileSystem;
using Snowflake.Model.Records.File;
using Snowflake.Records.Metadata;

namespace Snowflake.Model.Records
{
    public class FileRecord : IFileRecord
    {
        /// <inheritdoc/>
        public IMetadataCollection Metadata { get; }

        /// <inheritdoc/>
        public Guid RecordId => File.FileGuid;

        public IFile File { get; }

        /// <inheritdoc/>
        public string MimeType { get; }

        internal FileRecord(IFile file, string mimeType, IMetadataCollection metadataCollection)
        {
            this.MimeType = mimeType;
            this.File = file;
            this.Metadata = metadataCollection;
        }
    }
}

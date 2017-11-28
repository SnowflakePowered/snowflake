using Snowflake.Support.Remoting.GraphQl.Inputs.RecordMetadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Inputs.FileRecord
{
    public class FileRecordInputObject
    {
        public string FilePath { get; set; }
        public string MimeType { get; set; }
        public IList<RecordMetadataInputObject> Metadata { get; set; }
        public Guid Record { get; set; }
    }
}

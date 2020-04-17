using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Support.GraphQLFrameworkQueries.Inputs.RecordMetadata;

namespace Snowflake.Support.GraphQLFrameworkQueries.Inputs.FileRecord
{
    public class FileRecordInputObject
    {
        public string FilePath { get; set; }
        public string MimeType { get; set; }
        public IList<RecordMetadataInputObject> Metadata { get; set; }
    }
}

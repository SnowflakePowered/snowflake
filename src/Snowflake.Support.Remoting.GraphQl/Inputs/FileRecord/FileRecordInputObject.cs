﻿using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Support.Remoting.GraphQl.Inputs.RecordMetadata;

namespace Snowflake.Support.Remoting.GraphQl.Inputs.FileRecord
{
    public class FileRecordInputObject
    {
        public string FilePath { get; set; }
        public string MimeType { get; set; }
        public IList<RecordMetadataInputObject> Metadata { get; set; }
    }
}

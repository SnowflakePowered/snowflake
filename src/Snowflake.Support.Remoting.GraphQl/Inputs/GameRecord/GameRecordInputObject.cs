using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Support.Remoting.GraphQl.Inputs.RecordMetadata;

namespace Snowflake.Support.Remoting.GraphQl.Inputs.GameRecord
{
    public class GameRecordInputObject
    {
        public string Title { get; set; }
        public IList<RecordMetadataInputObject> Metadata { get; set; }
        public string Platform { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Inputs.Record
{
    public class GameRecordInputObject
    {
        public string Title { get; set; }
        public IList<RecordMetadataInputObject> Metadata { get; set; }
    }
}

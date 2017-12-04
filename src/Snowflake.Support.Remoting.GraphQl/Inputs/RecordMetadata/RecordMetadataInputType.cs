using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Inputs.RecordMetadata { 
    public class RecordMetadataInputType : InputObjectGraphType<RecordMetadataInputObject>
    {
        public RecordMetadataInputType()
        {
            Name = "RecordMetadataInput";
            Description = "An input type for a piece of record metadata.";
            Field(p => p.Key).Description("The metadata key.");
            Field(p => p.Value).Description("The metadata value.");
        }
    }
}

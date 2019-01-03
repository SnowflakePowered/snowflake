using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;

namespace Snowflake.Support.Remoting.GraphQL.Inputs.RecordMetadata
{
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

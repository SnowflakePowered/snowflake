using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Types.PlatformInfo
{
    internal class MetadataGraphType: ObjectGraphType<KeyValuePair<string, string>>
    {
        public MetadataGraphType()
        {
            Name = "Metadata";
            Field(p => p.Key).Description("The metadata key.");
            Field(p => p.Value).Description("The metadata value.");
        }
    }
}

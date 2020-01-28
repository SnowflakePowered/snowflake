using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;

namespace Snowflake.Support.GraphQLFrameworkQueries.Types.PlatformInfo
{
    internal class MetadataGraphType : ObjectGraphType<KeyValuePair<string, string>>
    {
        public MetadataGraphType()
        {
            Name = "Metadata";
            Field(p => p.Key).Description("The metadata key.");
            Field(p => p.Value).Description("The metadata value.");
        }
    }
}

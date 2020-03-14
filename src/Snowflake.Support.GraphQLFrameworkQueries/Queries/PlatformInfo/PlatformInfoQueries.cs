using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.PlatformInfo
{
    public class PlatformQueries
       : ObjectType<PlatformInfoQueryBuilder>
    {
        protected override void Configure(IObjectTypeDescriptor<PlatformInfoQueryBuilder> descriptor)
        {
            descriptor.Field(p => p.GetPlatforms())
                .UseFiltering<PlatformInfoFilter>()
                .Description("Gets the Stone Platforms definitions matching the search query.");
        }
    }

}

using HotChocolate.Types;
using Snowflake.Framework.Remoting.GraphQL.Schema;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.PlatformInfo
{
    public class PlatformQueries
       : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Query");
            descriptor.Field("platforms")
                .Resolver(context => 
                    context.Service<IStoneProvider>().Platforms.Values
                 )
                .UseFiltering<PlatformInfoFilter>()
                .Description("Gets the Stone Platforms definitions matching the search query.");
        }
    }

}

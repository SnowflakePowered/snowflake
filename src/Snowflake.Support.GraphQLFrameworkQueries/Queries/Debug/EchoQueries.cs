using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Debug
{
    public class EchoQueries
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor
                .Name("Query");
            descriptor.Field("DEBUG__HelloWorld")
                .Resolver("Hello World");
        }
    }
}

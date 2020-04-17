using HotChocolate.Language;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Runtime
{
    public class RuntimeQueryRoot {
        public static RuntimeQueryRoot Root = new RuntimeQueryRoot();
    }

    public sealed class RuntimeQueries
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor
                .Name("Query");
            descriptor.Field("runtime")
                .Type<NonNullType<RuntimeQueryType>>()
                .Description("Provides access to Snowflake runtime details.")
                .Resolver(RuntimeQueryRoot.Root);
        }
    }
}

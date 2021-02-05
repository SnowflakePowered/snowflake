using HotChocolate;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.GraphQL.Server
{
    internal sealed class DummyNodeType
        : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("_DummyNodeType")
                .Description("Dummy type to enable Relay support with only default types. Do not use this type.")
                .Field("id")
                .Deprecated("Do not use this type.")
                .Resolve("__dummy")
                .Type<IdType>();
            descriptor.AsNode()
                .NodeResolver<string>((ctx, id) => Task.FromResult(new object()));
        }
    }
}

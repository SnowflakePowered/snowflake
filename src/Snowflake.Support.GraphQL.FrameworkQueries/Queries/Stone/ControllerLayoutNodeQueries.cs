using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Snowflake.Input.Controller;
using Snowflake.Remoting.GraphQL;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Stone
{
    public sealed class ControllerLayoutNodeQueries
        : ObjectTypeExtension<IControllerLayout>
    {
        protected override void Configure(IObjectTypeDescriptor<IControllerLayout> descriptor)
        {
            descriptor.Name("ControllerLayout");
            descriptor.Implements<NodeType>();
            descriptor.Field("id")
                .Type<IdType>()
                .Resolve(ctx => ctx.Parent<IControllerLayout>().ControllerID);

            descriptor
                .ImplementsNode()
                .ResolveNode<string>((ctx, id) => Task.FromResult<object>(
                    ctx.SnowflakeService<IStoneProvider>().Controllers.TryGetValue(id, out var value) ? value : null));
        }
    }
}

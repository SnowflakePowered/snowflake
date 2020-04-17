using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Snowflake.Input.Controller;
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
            descriptor.Interface<NodeType>();
            descriptor.Field("id")
                .Type<IdType>()
                .Resolver(ctx => ctx.Parent<IControllerLayout>().ControllerID);

            descriptor.AsNode()
                .NodeResolver<ControllerId>((ctx, id) => Task.FromResult(
                    ctx.Service<IStoneProvider>().Controllers.TryGetValue(id, out var value) ? value : null));
        }
    }
}

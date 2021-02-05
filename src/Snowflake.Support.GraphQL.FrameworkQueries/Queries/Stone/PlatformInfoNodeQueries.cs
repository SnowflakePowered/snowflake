using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Snowflake.Model.Game;
using Snowflake.Remoting.GraphQL;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Stone
{
    public sealed class PlatformInfoNodeQueries
        : ObjectTypeExtension<IPlatformInfo>
    {
        protected override void Configure(IObjectTypeDescriptor<IPlatformInfo> descriptor)
        {
            descriptor.Name("PlatformInfo");
            descriptor.Interface<NodeType>();
            descriptor.Field("id")
                .Type<IdType>()
                .Resolve(ctx => ctx.Parent<IPlatformInfo>().PlatformID);

            descriptor
                .ImplementsNode()
                .ResolveNode<string>((ctx, id) => Task.FromResult<object>(
                    ctx.SnowflakeService<IStoneProvider>().Platforms.TryGetValue(id, out var value) ? value : null));
        }
    }
}

using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Snowflake.Model.Game;
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
                .Resolver(ctx => ctx.Parent<IPlatformInfo>().PlatformID);

            descriptor.AsNode()
                .NodeResolver<PlatformId>((ctx, id) => Task.FromResult(
                    ctx.Service<IStoneProvider>().Platforms.TryGetValue(id, out var value) ? value : null));
        }
    }
}

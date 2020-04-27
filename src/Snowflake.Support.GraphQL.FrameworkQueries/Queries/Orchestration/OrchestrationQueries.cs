using HotChocolate;
using HotChocolate.Types;
using Snowflake.Remoting.GraphQL;
using Snowflake.Remoting.GraphQL.Model.Orchestration;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Orchestration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Queries.Orchestration
{
    internal sealed class OrchestrationQueries
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.ExtendQuery();
            descriptor.Field("gameEmulation")
                .Description("Provides access to information about currently queued game emulation instances. Returns null if the instance does not exist.")
                .Argument("instanceId", arg => arg.Type<NonNullType<UuidType>>()
                    .Description("The `instanceId` of the game emulation instance that is used to query the game emulation state."))
                .Resolver(ctx =>
                {
                    var guid = ctx.Argument<Guid>("instanceId");
                    ctx.GetGameCache().TryGetValue(guid, out var gameEmulation);
                    return gameEmulation;
                })
                .Type<GameEmulationType>();
        }
    }
}

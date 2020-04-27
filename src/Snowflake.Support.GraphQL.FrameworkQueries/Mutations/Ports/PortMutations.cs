using HotChocolate;
using HotChocolate.Types;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Remoting.GraphQL;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Services;
using Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Ports
{
    public sealed class PortMutations
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Mutation");
            descriptor.Field("updatePortDevice")
                .UseAutoSubscription()
                .UseClientMutationId()
                .Description("Updates a virtual port device.")
                .Argument("input", arg => arg.Type<NonNullType<UpdatePortDeviceInputType>>())
                .Resolver(ctx =>
                {
                    var input = ctx.Argument<UpdatePortDeviceInput>("input");
                    var orchestrator = ctx.SnowflakeService<IPluginManager>().GetCollection<IEmulatorOrchestrator>()[input.Orchestrator];
                    if (orchestrator == null)
                        return ErrorBuilder.New()
                               .SetCode("PORT_NOTFOUND_ORCHESTRATOR")
                               .SetMessage("The specified orchestrator was not found.")
                               .Build();
                    ctx.SnowflakeService<IEmulatedPortStore>()
                        .SetPort(orchestrator, input.PlatformID, input.PortIndex, input.ControllerID,
                            input.InstanceID, input.Driver, input.ProfileID);
                    return new UpdatePortPayload()
                    {
                        PortEntry = ctx.SnowflakeService<IEmulatedPortStore>().GetPort(orchestrator, input.PlatformID, input.PortIndex),
                    };
                })
                .Type<NonNullType<UpdatePortPayloadType>>();
            descriptor.Field("clearPortDevice")
                .UseAutoSubscription()
                .UseClientMutationId()
                .Argument("input", arg => arg.Type<NonNullType<ClearPortDeviceInputType>>())
                .Resolver(ctx =>
                {
                    var input = ctx.Argument<ClearPortDeviceInput>("input");
                    var orchestrator = ctx.SnowflakeService<IPluginManager>().GetCollection<IEmulatorOrchestrator>()[input.Orchestrator];
                    if (orchestrator == null)
                        return ErrorBuilder.New()
                               .SetCode("PORT_NOTFOUND_ORCHESTRATOR")
                               .SetMessage("The specified orchestrator was not found.")
                               .Build();
                    var oldPort = ctx.SnowflakeService<IEmulatedPortStore>().GetPort(orchestrator, input.PlatformID, input.PortIndex);

                    ctx.SnowflakeService<IEmulatedPortStore>()
                        .ClearPort(orchestrator, input.PlatformID, input.PortIndex);
                    return new ClearPortPayload()
                    {
                        PortEntry = oldPort,
                    };
                })
                .Type<NonNullType<ClearPortPayloadType>>();
        }
    }
}

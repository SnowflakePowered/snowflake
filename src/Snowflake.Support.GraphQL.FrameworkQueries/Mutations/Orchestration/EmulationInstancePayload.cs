using HotChocolate.Types;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Remoting.GraphQL.Model.Orchestration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Orchestration
{
    public sealed class EmulationInstancePayload
        : RelayMutationBase
    {
        public Guid InstanceID { get; set; }
        public IGameEmulation GameEmulation { get; set; }
    }

    public sealed class EmulationInstancePayloadType
        : ObjectType<EmulationInstancePayload>
    {
        protected override void Configure(IObjectTypeDescriptor<EmulationInstancePayload> descriptor)
        {
            descriptor.Name(nameof(EmulationInstancePayload))
                .WithClientMutationId();

            descriptor.Field(i => i.InstanceID)
                .Name("instanceId")
                .Description("The GUID of the emulation instance to use as a handle to modify the instance.")
                .Type<NonNullType<UuidType>>();
            descriptor.Field(i => i.GameEmulation)
                .Description("The emulation instance that was updated.")
                .Type<NonNullType<GameEmulationType>>();
        }
    }
}

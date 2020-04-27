using HotChocolate.Types;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Remoting.GraphQL.Model.Orchestration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Orchestration
{
    public sealed class CleanupEmulationPayload
        : RelayMutationBase
    {
        public Guid InstanceID { get; set; }
        public bool Success { get; set; }
    }

    public sealed class CleanupEmulationPayloadType
        : ObjectType<CleanupEmulationPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<CleanupEmulationPayload> descriptor)
        {
            descriptor.Name(nameof(CleanupEmulationPayload))
                .WithClientMutationId();

            descriptor.Field(i => i.InstanceID)
                .Name("instanceId")
                .Description("The instance that was cleaned up.")
                .Type<NonNullType<UuidType>>();
            descriptor.Field(i => i.Success)
                .Description("Whether or not the cleanup was a success.")
                .Type<NonNullType<BooleanType>>();
        }
    }
}

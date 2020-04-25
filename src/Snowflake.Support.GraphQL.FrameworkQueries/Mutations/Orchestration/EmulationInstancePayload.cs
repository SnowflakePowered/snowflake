using HotChocolate.Types;
using Snowflake.Orchestration.Extensibility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Orchestration
{
    public sealed class EmulationInstancePayload
    {
        public Guid InstanceID { get; set; }
        public IGameEmulation Emulation { get; set; }
    }

    public sealed class EmulationInstancePayloadType
        : ObjectType<EmulationInstancePayload>
    {
        protected override void Configure(IObjectTypeDescriptor<EmulationInstancePayload> descriptor)
        {
            descriptor.Name(nameof(EmulationInstancePayload));
            // todo game emulation query type

            descriptor.Field(i => i.InstanceID)
                .Name("instanceId")
                .Type<NonNullType<UuidType>>();
        }
    }
}

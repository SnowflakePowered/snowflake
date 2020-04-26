using HotChocolate.Types;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Support.GraphQL.FrameworkQueries.Queries.Game.Orchestration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Ports
{
    public sealed class ClearPortPayload
    {
        public IEmulatedPortDeviceEntry PortEntry { get; set; }
    }

    public sealed class ClearPortPayloadType
        : ObjectType<ClearPortPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<ClearPortPayload> descriptor)
        {
            descriptor.Name(nameof(ClearPortPayload));
            descriptor.Field(p => p.PortEntry)
                .Description("The port entry that was cleared.")
                .Type<ConnectedEmulatedControllerType>();
        }
    }
}

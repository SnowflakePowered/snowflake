using HotChocolate.Types;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Support.GraphQL.FrameworkQueries.Queries.Game.Orchestration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Ports
{
    public sealed class UpdatePortPayload
    {
        public IEmulatedPortDeviceEntry PortEntry { get; set; }
    }

    public sealed class UpdatePortPayloadType
        : ObjectType<UpdatePortPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<UpdatePortPayload> descriptor)
        {
            descriptor.Name(nameof(UpdatePortPayload));
            descriptor.Field(p => p.PortEntry)
                .Description("The port entry that was updated")
                .Type<ConnectedEmulatedControllerType>();
        }
    }
}

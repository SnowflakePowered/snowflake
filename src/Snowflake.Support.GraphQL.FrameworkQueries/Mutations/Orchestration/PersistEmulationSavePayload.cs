using HotChocolate.Types;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Orchestration.Saving;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Remoting.GraphQL.Model.Orchestration;
using Snowflake.Remoting.GraphQL.Model.Saving;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Orchestration
{
    public sealed class PersistEmulationSavePayload
        : RelayMutationBase
    {
        public Guid InstanceID { get; set; }
        public IGameEmulation GameEmulation { get; set; }
        public ISaveGame SaveGame { get; set; }
    }

    public sealed class PersistEmulationSavePayloadType
        : ObjectType<PersistEmulationSavePayload>
    {
        protected override void Configure(IObjectTypeDescriptor<PersistEmulationSavePayload> descriptor)
        {
            descriptor.Name(nameof(PersistEmulationSavePayload))
                .WithClientMutationId();

            descriptor.Field(i => i.InstanceID)
                .Name("instanceId")
                .Description("The GUID of the emulation instance to use as a handle to modify the instance.")
                .Type<NonNullType<UuidType>>();
            descriptor.Field(i => i.GameEmulation)
                .Description("The emulation instance that was updated.")
                .Type<NonNullType<GameEmulationType>>();
            descriptor.Field(i => i.SaveGame)
                .Description("The save game that was written.")
                .Type<NonNullType<SaveGameType>>();
        }
    }
}

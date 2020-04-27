using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Orchestration
{
    internal sealed class CreateEmulationInstanceInput
        : RelayMutationBase
    {
        public string Orchestrator { get; set; }
        public Guid GameID { get; set; }
        public Guid CollectionID { get; set; }
        public Guid SaveProfileID { get; set; }
    }

    internal sealed class CreateEmulationInstanceInputType
        : InputObjectType<CreateEmulationInstanceInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateEmulationInstanceInput> descriptor)
        {
            descriptor.Name(nameof(CreateEmulationInstanceInput))
                .WithClientMutationId();

            descriptor.Field(i => i.Orchestrator)
                .Description("The name of the orchestrator to use to launch this emulation instance.")
                .Type<NonNullType<StringType>>();

            descriptor.Field(i => i.GameID)
                .Name("gameId")
                .Description("The `gameId` GUID of the game to create the instance for.")
                .Type<NonNullType<UuidType>>();

            descriptor.Field(i => i.CollectionID)
                .Name("collectionId")
                .Description("The `collectionId` of the configuration collection to use for this emulation instance.")
                .Type<NonNullType<UuidType>>();

            descriptor.Field(i => i.SaveProfileID)
                .Name("saveProfileId")
                .Description("The `profileId` of the save profile to use for this emulation instance.")
                .Type<NonNullType<UuidType>>();
        }
    }
}

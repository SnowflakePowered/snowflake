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
                .Type<NonNullType<StringType>>();

            descriptor.Field(i => i.GameID)
                .Name("gameId")
                .Type<NonNullType<UuidType>>();

            descriptor.Field(i => i.CollectionID)
                .Name("collectionId")
                .Type<NonNullType<UuidType>>();

            descriptor.Field(i => i.SaveProfileID)
                .Name("saveProfileId")
                .Type<NonNullType<UuidType>>();
        }
    }
}

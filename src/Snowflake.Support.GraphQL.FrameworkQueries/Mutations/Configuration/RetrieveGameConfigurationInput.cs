using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Configuration
{
    internal sealed class RetrieveGameConfigurationInput
    {
        public Guid GameID { get; set; }
        public string Orchestrator { get; set; }
    }

    internal sealed class RetrieveGameConfigurationInputType
        : InputObjectType<RetrieveGameConfigurationInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RetrieveGameConfigurationInput> descriptor)
        {
            descriptor.Name(nameof(RetrieveGameConfigurationInput))
                .Description("Describes a game configuration by its game ID, orchestrator.");

            descriptor.Field(i => i.GameID)
                .Name("gameId")
                .Type<NonNullType<UuidType>>();

            descriptor.Field(i => i.Orchestrator)
                .Type<NonNullType<StringType>>();
        }
    }
}

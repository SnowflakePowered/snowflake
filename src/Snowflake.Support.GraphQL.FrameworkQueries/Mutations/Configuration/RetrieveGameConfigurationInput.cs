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
            descriptor.Name(nameof(RetrieveGameConfigurationInput));

            descriptor.Field(i => i.GameID)
                .Name("gameId")
                .Description("The `gameId` GUID of the game the configuration belongs to.")
                .Type<NonNullType<UuidType>>();

            descriptor.Field(i => i.Orchestrator)
                .Description("The name of the orchestrator plugin that the configuration is from.")
                .Type<NonNullType<StringType>>();
        }
    }
}

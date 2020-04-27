using HotChocolate.Types;
using Snowflake.Orchestration.Saving;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Remoting.GraphQL.Model.Saving;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Saving
{
    internal sealed class CreateSaveProfileInput
        : RelayMutationBase
    {
        public Guid GameID { get; set; }
        public string ProfileName { get; set; }
        public string SaveType { get; set; }
        public SaveManagementStrategy ManagementStrategy { get; set; }
    }

    internal sealed class CreateSaveProfileInputType
        : InputObjectType<CreateSaveProfileInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateSaveProfileInput> descriptor)
        {
            descriptor.Name(nameof(CreateSaveProfileInput))
                .WithClientMutationId();

            descriptor.Field(g => g.GameID)
                .Name("gameId")
                .Description("The `gameId` GUID of the game to create a save profile for.")
                .Type<NonNullType<UuidType>>();

            descriptor.Field(g => g.ProfileName)
                .Description("The name of the new save profile.")
                .Type<NonNullType<StringType>>();

            descriptor.Field(g => g.SaveType)
                .Description("The save type of the new save profile.")
                .Type<NonNullType<StringType>>();

            descriptor.Field(g => g.ManagementStrategy)
                .Description("The management strategy this save profile will use to manage subsequent saves.")
                .Type<NonNullType<SaveManagementStrategyEnum>>();
        }
    }
}
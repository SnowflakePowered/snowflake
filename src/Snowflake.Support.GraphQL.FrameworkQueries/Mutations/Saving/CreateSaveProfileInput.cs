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
                .Type<NonNullType<UuidType>>();

            descriptor.Field(g => g.ProfileName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(g => g.SaveType)
                .Type<NonNullType<StringType>>();

            descriptor.Field(g => g.ManagementStrategy)
                .Type<NonNullType<SaveManagementStrategyEnum>>();
        }
    }
}

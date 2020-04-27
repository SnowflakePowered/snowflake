using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.Model.Stone.PlatformInfo;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Game
{
    internal sealed class DeleteGameInput
        : RelayMutationBase
    {
        public Guid GameID { get; set; }
    }

    internal sealed class DeleteGameInputType
        : InputObjectType<DeleteGameInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteGameInput> descriptor)
        {
            descriptor.Name(nameof(DeleteGameInput))
                .WithClientMutationId();

            descriptor.Field(i => i.GameID)
                .Name("gameId")
                .Description("The `gameId` GUID of the game to delete.")
                .Type<NonNullType<UuidType>>();
        }
    }
}

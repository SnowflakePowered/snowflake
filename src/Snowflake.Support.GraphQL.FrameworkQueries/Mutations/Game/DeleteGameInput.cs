using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.Model.Stone.PlatformInfo;
using Snowflake.Remoting.GraphQL.RelayMutations;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;

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
                .Type<NonNullType<UuidType>>();
        }
    }
}

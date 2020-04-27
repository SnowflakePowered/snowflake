using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.Model.Stone.PlatformInfo;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Game
{
    internal sealed class DeleteGameMetadataInput
        : RelayMutationBase
    {
        public Guid GameID { get; set; }
        public string MetadataKey { get; set; }
    }

    internal sealed class DeleteGameMetadataInputType
        : InputObjectType<DeleteGameMetadataInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteGameMetadataInput> descriptor)
        {
            descriptor.Name(nameof(DeleteGameMetadataInput))
                .WithClientMutationId();

            descriptor.Field(i => i.GameID)
                .Name("gameId")
                .Description("The `gameId` GUID of the game that owns the metadata to delete.")
                .Type<NonNullType<UuidType>>();

            descriptor.Field(i => i.MetadataKey)
                .Description("The metadata key of the metadata to delete.")
                .Type<NonNullType<StringType>>();
        }
    }
}

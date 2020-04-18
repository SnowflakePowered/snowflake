using HotChocolate.Types;
using Snowflake.Framework.Remoting.GraphQL.Model.Stone.PlatformInfo;
using Snowflake.Framework.Remoting.GraphQL.RelayMutations;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;

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
                .Type<NonNullType<UuidType>>();

            descriptor.Field(i => i.MetadataKey)
               .Type<NonNullType<StringType>>();
        }
    }
}

using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.Model.Stone.PlatformInfo;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Game
{
    internal sealed class UpdateGameMetadataInput
        : RelayMutationBase
    {
        public Guid GameID { get; set; }
        public string MetadataKey { get; set; }
        public string MetadataValue { get; set; }
    }

    internal sealed class UpdateGameMetadataInputType
        : InputObjectType<UpdateGameMetadataInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateGameMetadataInput> descriptor)
        {
            descriptor.Name(nameof(UpdateGameMetadataInput))
                .WithClientMutationId();

            descriptor.Field(i => i.GameID)
                .Name("gameId")
                .Type<NonNullType<UuidType>>();

            descriptor.Field(i => i.MetadataKey)
               .Type<NonNullType<StringType>>();

            descriptor.Field(i => i.MetadataValue)
               .Type<NonNullType<StringType>>();
        }
    }
}

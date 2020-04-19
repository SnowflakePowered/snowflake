using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.Model.Records;
using Snowflake.Remoting.GraphQL.RelayMutations;
using Snowflake.Model.Game;
using Snowflake.Model.Records;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Game
{
    internal sealed class UpdateGameMetadataPayload
        : RelayMutationBase
    {
        public IGame Game { get; set; }
        public IRecordMetadata Metadata { get; set; }
    }

    internal sealed class UpdateGameMetadataPayloadType
        : ObjectType<UpdateGameMetadataPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<UpdateGameMetadataPayload> descriptor)
        {
            descriptor.Name("UpdateGameMetadataPayload")
                .WithClientMutationId();

            descriptor.Field(p => p.Game)
                .Type<NonNullType<GameType>>();
            descriptor.Field(p => p.Metadata)
               .Type<NonNullType<RecordMetadataType>>();
        }
    }
}

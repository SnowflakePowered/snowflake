using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.Model.Records;
using Snowflake.Model.Game;
using Snowflake.Model.Records;
using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Game
{
    internal sealed class DeleteGameMetadataPayload
        : RelayMutationBase
    {
        public IGame Game { get; set; }
        public IRecordMetadata Metadata { get; set; }
    }

    internal sealed class DeleteGameMetadataPayloadType
        : ObjectType<DeleteGameMetadataPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<DeleteGameMetadataPayload> descriptor)
        {
            descriptor.Name("DeleteGameMetadataPayload")
                .WithClientMutationId();

            descriptor.Field(p => p.Game)
                .Type<NonNullType<GameType>>();
            descriptor.Field(p => p.Metadata)
               .Type<RecordMetadataType>();
        }
    }
}

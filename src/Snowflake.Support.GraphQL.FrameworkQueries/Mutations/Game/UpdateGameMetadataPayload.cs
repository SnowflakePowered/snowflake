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
                .Description("The game that metadata was modified for.")
                .Type<NonNullType<GameType>>();
            descriptor.Field(p => p.Metadata)
                .Description("The modified metadata.")
                .Type<NonNullType<RecordMetadataType>>();
        }
    }
}

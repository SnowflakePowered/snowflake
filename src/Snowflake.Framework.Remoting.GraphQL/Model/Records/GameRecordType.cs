using HotChocolate.Types;
using Snowflake.Framework.Remoting.GraphQL.Model.Records;
using Snowflake.Framework.Remoting.GraphQL.Model.Stone.PlatformInfo;
using Snowflake.Model.Records.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Game
{
    public sealed class GameRecordType
        : ObjectType<IGameRecord>
    {
        protected override void Configure(IObjectTypeDescriptor<IGameRecord> descriptor)
        {
            descriptor.Name("GameRecord")
                .BindFieldsExplicitly()
                .Description("The record associated with a Game and its associated metadata.");

            descriptor.Field(g => g.Title)
                .Description("The title of the game.");

            descriptor.Field(g => g.PlatformID)
                .Type<PlatformIdType>()
                .Description("The original platform or game console of the game this object represents.");

            descriptor.Field(g => g.RecordID)
                .Description("The unique ID of the game.");

            descriptor.Field("metadata")
                .Description("The metadata associated with this game.")
                .Resolver(ctx => ctx.Parent<IGameRecord>().Metadata.Select(m => m.Value))
                .Type<ListType<RecordMetadataType>>()
                .UseFiltering();
        }
    }
}

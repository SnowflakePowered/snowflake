using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.Model.Records;
using Snowflake.Remoting.GraphQL.Model.Stone.PlatformInfo;
using Snowflake.Model.Records.Game;
using System.Linq;
using Snowflake.Framework.Remoting.GraphQL.Records.Filters;
using HotChocolate.Data.Filters;

namespace Snowflake.Remoting.GraphQL.Model.Game
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
                .Name("platformId")
                .Type<NonNullType<PlatformIdType>>()
                .Description("The original platform or game console of the game this object represents.");

            descriptor.Field(g => g.RecordID)
                .Name("recordId")
                .Description("The unique ID of the game.");

            descriptor.Field("metadata")
                .Description("The metadata associated with this game.")
                .Resolve(ctx => ctx.Parent<IGameRecord>().Metadata.Select(m => m.Value))
                .Type<NonNullType<ListType<NonNullType<RecordMetadataType>>>>()
                .UseFiltering<ListFilterInputType<MetadataFilterInputType>>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;
using Snowflake.Records.Game;

namespace Snowflake.Support.Remoting.GraphQl.Types.Record
{
    public class GameRecordGraphType : ObjectGraphType<IGameRecord>
    {
        public GameRecordGraphType()
        {
            Name = "GameRecord";
            Description = "A record of an executable Game";
            Field<ListGraphType<FileRecordGraphType>>(
                "files",
                description: "A list of files associated with this game.",
                resolve: context => context.Source.Files);
            Field(g => g.PlatformID).Description("The ID of the platform that this game is from.");
            Field(g => g.Title).Description("The title of the game.");
            Field<GuidGraphType>("guid",
               description: "The unique ID of the game.",
               resolve: context => context.Source.Guid);
            Field<StringGraphType>("id",
             description: "The opaque GraphQL unique ID of the game. For caching purposes only.",
             resolve: context => context.Source.Guid.ToGraphQlUniqueId("GameRecord"));
            Field<ListGraphType<RecordMetadataGraphType>>(
                "metadata",
                description: "A list of metadata related to this game.",
                resolve: context => context.Source.Metadata.Select(m => m.Value));
            Interface<RecordInterface>();
        }
    }
}

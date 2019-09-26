using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;
using Snowflake.Model.Records.Game;

namespace Snowflake.Support.Remoting.GraphQL.Types.Model
{
    public class GameRecordGraphType : ObjectGraphType<IGameRecord>
    {
        public GameRecordGraphType()
        {
            Name = "GameRecord";
            Description = "A record of an executable Game";

            //Field<ListGraphType<FileRecordGraphType>>(
            //    "files",
            //    description: "A list of files associated with this game.",
            //    resolve: context => context.Source.WithFiles().FileRecords);

            Field(g => g.Title, nullable: true).Description("The title of the game.");

            Field<StringGraphType>("platformId",
                description: "The platform or console this game was created for",
                resolve: context => context.Source.PlatformID.ToString());

            Field<GuidGraphType>("guid",
                description: "The unique ID of the game.",
                resolve: context => context.Source.RecordID);

            Field<StringGraphType>("id",
                description: "The opaque GraphQL unique ID of the game. For caching purposes only.",
                resolve: context => context.Source.RecordID.ToGraphQlUniqueId("GameRecord"));

            Field<ListGraphType<RecordMetadataGraphType>>(
                "metadata",
                description: "A list of metadata related to this game.",
                resolve: context => context.Source.Metadata.Select(m => m.Value));
            Interface<RecordInterface>();
        }
    }
}

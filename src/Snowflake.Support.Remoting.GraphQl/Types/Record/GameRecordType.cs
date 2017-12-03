using GraphQL.Conventions.Adapters.Types;
using GraphQL.Types;
using Snowflake.Records.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Types.Record
{
    public class GameRecordType : ObjectGraphType<IGameRecord>
    {
        public GameRecordType()
        {
            Name = "GameRecord";
            Description = "A record of an executable Game";
            Field<ListGraphType<FileRecordType>>(
                "files",
                description: "A list of files associated with this game.",
                resolve: context => context.Source.Files
            );
            Field(g => g.PlatformID).Description("The ID of the platform that this game is from.");
            Field(g => g.Title).Description("The title of the game.");
            Field<GuidGraphType>("guid",
               description: "The unique ID of the game.",
               resolve: context => context.Source.Guid);
            Field<ListGraphType<RecordMetadataType>>(
                "metadata",
                description: "A list of metadata related to this game.",
                resolve: context => context.Source.Metadata.Select(m => m.Value)
                );
            Interface<RecordInterface>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;
using Snowflake.Support.Remoting.GraphQl.Inputs.RecordMetadata;

namespace Snowflake.Support.Remoting.GraphQl.Inputs.GameRecord
{
    public class GameRecordInputType : InputObjectGraphType<GameRecordInputObject>
    {
        public GameRecordInputType()
        {
            Name = "GameRecordInputType";
            Field(p => p.Title).Description("The title of the game.");
            Field<NonNullGraphType<ListGraphType<RecordMetadataInputType>>>("metadata",
                description: "Some metadata about the game.",
                resolve: context => context.Source.Metadata);
            Field(p => p.Platform).Description("The platform of the game.");
        }
    }
}

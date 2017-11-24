using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Inputs.Record { 
    public class GameRecordInputType : InputObjectGraphType<GameRecordInputObject>
    {
        public GameRecordInputType()
        {
            Name = "GameRecordInputType";
            Field(p => p.Title).Description("The title of the game.");
            Field<ListGraphType<RecordMetadataInputType>>("metadata",
                description: "Some metadata about the game.",
                resolve: context => context.Source.Metadata
            );
        }
    }
}

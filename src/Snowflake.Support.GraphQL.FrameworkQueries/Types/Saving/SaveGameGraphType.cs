using GraphQL.Types;
using Snowflake.Orchestration.Saving;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Types.Saving
{
    public class SaveGameGraphType : ObjectGraphType<ISaveGame>
    {
        public SaveGameGraphType()
        {
            Field<StringGraphType>("saveType", resolve: c => c.Source.SaveType, description: "The type of the save.");
            Field<DateTimeOffsetGraphType>("createdTimestamp", resolve: c => c.Source.CreatedTimestamp,
                description: "The timestamp of when this save was created.");
        }
    }
}

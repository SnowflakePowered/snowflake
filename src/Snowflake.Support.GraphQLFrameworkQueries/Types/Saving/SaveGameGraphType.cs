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
            Field<ListGraphType<StringGraphType>>("tags", resolve: c => c.Source.Tags, description: "Any tags this save has.");
            Field<GuidGraphType>("saveGuid", resolve: c => c.Source.Guid, description: "The guid of this save.");
            Field<DateTimeOffsetGraphType>("createdTime", resolve: c => c.Source.CreatedTimestamp,
                description: "The timestamp of when this save was created.");
        }
    }
}

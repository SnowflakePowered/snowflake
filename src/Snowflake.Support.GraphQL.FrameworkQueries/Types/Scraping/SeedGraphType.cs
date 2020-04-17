using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Scraping;

namespace Snowflake.Support.GraphQLFrameworkQueries.Types.Scraping
{
    public class SeedGraphType : ObjectGraphType<ISeed>
    {
        public SeedGraphType()
        {
            Name = "Seed";
            Description = "A seed represents a piece of data in the process of scraping a game.";
            Field(s => s.Source).Description("The source of this seed.");
            Field<GuidGraphType>("guid",
                resolve: context => context.Source.Guid);
            Field<GuidGraphType>("parent",
                resolve: context => context.Source.Parent);
            Field<SeedContentGraphType>("content",
                resolve: context => context.Source.Content);
        }
    }
}

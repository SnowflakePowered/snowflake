using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Scraping;

namespace Snowflake.Support.Remoting.GraphQL.Types.Scraping
{
    public class SeedContentGraphType : ObjectGraphType<SeedContent>
    {
        public SeedContentGraphType()
        {
            Name = "SeedContent";
            Description = "A content of the seed.";
            Field(s => s.Type).Description("The type of the Seed content.");
            Field(s => s.Value).Description("The value of the Seed content.");
        }
    }
}

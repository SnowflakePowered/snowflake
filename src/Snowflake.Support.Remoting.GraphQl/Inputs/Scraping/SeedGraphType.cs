using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Conventions.Adapters.Types;
using GraphQL.Types;
using Snowflake.Scraping;

namespace Snowflake.Support.Remoting.GraphQl.Inputs.Scraping
{
    public class SeedGraphType : ObjectGraphType<ISeed>
    {
        public SeedGraphType()
        {
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

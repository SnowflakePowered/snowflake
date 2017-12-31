using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;

namespace Snowflake.Support.Remoting.GraphQl.Inputs.Scraping
{
    public class SeedTreeInputType : InputObjectGraphType<SeedTreeInputObject>
    {
        public SeedTreeInputType()
        {
            Name = "SeedTreeInput";
            Field(s => s.Type);
            Field(s => s.Value);
            Field<ListGraphType<SeedTreeInputType>>("children",
                resolve: context => context.Source.Children);
        }
    }
}

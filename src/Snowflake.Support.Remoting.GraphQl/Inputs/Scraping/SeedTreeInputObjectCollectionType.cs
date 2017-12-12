using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;

namespace Snowflake.Support.Remoting.GraphQl.Inputs.Scraping
{
    public class SeedTreeInputObjectCollectionType : InputObjectGraphType<SeedTreeInputObjectCollection>
    {
        public SeedTreeInputObjectCollectionType()
        {
            Field<ListGraphType<SeedTreeInputType>>("seeds",
                resolve: context => context.Source.Seeds);
        }
    }
}

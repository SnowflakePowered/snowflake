using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;

namespace Snowflake.Support.GraphQLFrameworkQueries.Inputs.Scraping
{
    public class SeedTreeInputObjectCollectionType : InputObjectGraphType<SeedTreeInputObjectCollection>
    {
        public SeedTreeInputObjectCollectionType()
        {
            Name = "SeedTreeInputObjectCollection";
            Field<ListGraphType<SeedTreeInputType>>("seeds",
                resolve: context => context.Source.Seeds);
        }
    }
}

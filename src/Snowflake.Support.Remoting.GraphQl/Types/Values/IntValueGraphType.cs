using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Types.Values
{
    public class IntValueGraphType : ObjectGraphType<int>
    {
        public IntValueGraphType()
        {
            Name = "IntValue";
            Description = "Boxes a `Int` scalar into an ObjectGraphType.";
            Field<IntGraphType>("value",
                resolve: context => context.Source);
        }
    }
}

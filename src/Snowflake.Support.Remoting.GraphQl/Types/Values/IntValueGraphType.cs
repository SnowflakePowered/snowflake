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
            Field<IntGraphType>("value",
                resolve: context => context.Source);
        }
    }
}

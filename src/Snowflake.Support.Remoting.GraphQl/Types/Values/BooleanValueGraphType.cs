using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Types.Values
{
    public class BooleanValueGraphType : ObjectGraphType<bool>
    {
        public BooleanValueGraphType()
        {
            Field<BooleanGraphType>("value",
                resolve: context => context.Source);
        }
    }
}

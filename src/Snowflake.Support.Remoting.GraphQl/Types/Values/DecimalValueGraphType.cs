using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Types.Values
{
    public class DecimalValueGraphType : ObjectGraphType<double>
    {
        public DecimalValueGraphType()
        {
            Field<FloatGraphType>("value",
                resolve: context => context.Source);
        }
    }
}

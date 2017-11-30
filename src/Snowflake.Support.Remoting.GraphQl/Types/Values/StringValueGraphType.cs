using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Types.Values
{
    public class StringValueGraphType : ObjectGraphType<string>
    {
        public StringValueGraphType()
        {
            Field<StringGraphType>("value",
                resolve: context => context.Source);
        }
    }
}

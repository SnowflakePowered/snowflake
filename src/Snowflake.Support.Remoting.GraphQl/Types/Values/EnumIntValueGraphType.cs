using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Types.Values
{
    public class EnumIntValueGraphType : ObjectGraphType<Enum>
    {
        public EnumIntValueGraphType()
        {
            Field<IntGraphType>("value",
                resolve: context => Convert.ToInt32(context.Source));
            Field<StringGraphType>("name",
                resolve: context => Enum.GetName(context.Source.GetType(), context.Source));
        }
    }
}

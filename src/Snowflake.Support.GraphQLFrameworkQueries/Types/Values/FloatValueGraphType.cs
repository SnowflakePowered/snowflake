using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;

namespace Snowflake.Support.Remoting.GraphQL.Types.Values
{
    public class FloatValueGraphType : ObjectGraphType<double>
    {
        public FloatValueGraphType()
        {
            Name = "FloatValue";
            Description = "Boxes a `Float` scalar into an ObjectGraphType.";
            Field<FloatGraphType>("value",
                resolve: context => context.Source);
        }
    }
}

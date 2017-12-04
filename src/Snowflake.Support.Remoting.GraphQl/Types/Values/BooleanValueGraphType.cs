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
            Name = "BooleanValue";
            Description = "Boxes a `Boolean` scalar into an ObjectGraphType.";
            Field<BooleanGraphType>("value",
                resolve: context => context.Source);
        }
    }
}

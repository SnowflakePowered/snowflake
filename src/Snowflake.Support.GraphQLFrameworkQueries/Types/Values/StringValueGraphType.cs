using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;

namespace Snowflake.Support.Remoting.GraphQL.Types.Values
{
    public class StringValueGraphType : ObjectGraphType<string>
    {
        public StringValueGraphType()
        {
            Name = "StringValue";
            Description = "Boxes a `String` scalar into an ObjectGraphType.";
            Field<StringGraphType>("value",
                resolve: context => context.Source);
        }
    }
}

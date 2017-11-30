using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Language.AST;

namespace Snowflake.Support.Remoting.GraphQl.Types.Values
{
    public class PrimitiveGraphType : ScalarGraphType
    {
        public PrimitiveGraphType()
        {
            Name = "Primitive";
            Description = "A type-unsafe primitive value.";
        }
        public override object ParseLiteral(IValue value)
        {
            return value.Value;
        }

        public override object ParseValue(object value)
        {
            return value;
        }

        public override object Serialize(object value)
        {
            return ParseValue(value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Language.AST;
using GraphQL.Types;

namespace Snowflake.Support.Remoting.GraphQl.Types.Values
{
    public class PrimitiveGraphType : ScalarGraphType
    {
        public PrimitiveGraphType()
        {
            Name = "Primitive";
            Description = "A type-unsafe scalar value.";
        }

        /// <inheritdoc/>
        public override object ParseLiteral(IValue value)
        {
            return value.Value;
        }

        /// <inheritdoc/>
        public override object ParseValue(object value)
        {
            return value;
        }

        /// <inheritdoc/>
        public override object Serialize(object value)
        {
            return value;
        }
    }
}

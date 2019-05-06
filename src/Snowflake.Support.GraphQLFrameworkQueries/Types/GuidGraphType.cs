using System;
using GraphQL.Language.AST;
using GraphQL.Types;

namespace GraphQL.Types
{
    public class GuidGraphType : ScalarGraphType
    {
        public GuidGraphType()
        {
            Name = "Guid";
            Description = "Globally Unique Identifier.";
        }

        public override object ParseValue(object value) =>
            ValueConverter.ConvertTo(value, typeof(Guid));

        public override object Serialize(object value) => ParseValue(value);

        /// <inheritdoc/>
        public override object ParseLiteral(IValue value)
        {
            if (value is GuidValue guidValue)
            {
                return guidValue.Value;
            }

            if (value is StringValue str)
            {
                return ParseValue(str.Value);
            }

            return null;
        }
    }
}

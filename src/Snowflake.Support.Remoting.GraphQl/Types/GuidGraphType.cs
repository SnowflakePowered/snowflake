using System;
using GraphQL.Language.AST;
using GraphQL.Types;

namespace GraphQL.Conventions.Adapters.Types
{
    public class GuidGraphType : ScalarGraphType
    {
        public GuidGraphType()
        {
            Name = "Guid";
            Description = "Globally Unique Identifier.";
        }

        public override object Serialize(object value)
        {
            return value?.ToString();
        }

        public override object ParseValue(object value)
        {
            var guid = value?.ToString().StripQuotes();
            return string.IsNullOrWhiteSpace(guid)
                ? null
                : (Guid?)Guid.Parse(guid);
        }

        public override object ParseLiteral(IValue value)
        {
            var str = value as StringValue;
            if (str != null)
            {
                return ParseValue(str.Value);
            }
            return null;
        }
    }
}

internal static class StringExtensions
{
    public static string StripQuotes(this string value)
    {
        if (!string.IsNullOrEmpty(value) && value.Length > 2 && value.StartsWith("\"") && value.EndsWith("\""))
        {
            return value.Substring(1, value.Length - 2);
        }
        return value;
    }
}
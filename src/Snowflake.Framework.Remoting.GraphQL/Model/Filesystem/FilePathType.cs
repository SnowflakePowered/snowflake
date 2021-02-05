using HotChocolate.Language;
using HotChocolate.Types;
using Snowflake.Input.Controller;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Zio;

namespace Snowflake.Remoting.GraphQL.Model.Filesystem
{
    /// <summary>
    /// GraphQL Scalar Definition
    /// </summary>
    public sealed class FilePathType
    : ScalarType<UPath, StringValueNode>
    {
        /// <summary>
        /// GraphQL Scalar Definition for a <see cref="UPath"/> pointing to a directory
        /// </summary>
        public FilePathType()
            : base("FilePath", BindingBehavior.Implicit)
        {
            Description = "Represents a unix-like relocatable contextual path string. This is not an OS-dependent path string. Paths of this type point to files in the Schema, but there is no guarantee of verification.";
        }

        protected override UPath ParseLiteral(StringValueNode literal)
        {
            if (Path.EndsInDirectorySeparator(literal.Value))
                throw new ArgumentException("File paths can not end in the directory separator character.");
            return (UPath)literal.Value;
        }

        protected override StringValueNode ParseValue(UPath value)
        {
            return new StringValueNode(null, value.FullName, false);
        }

        public override IValueNode ParseResult(object resultValue) => ParseValue(resultValue);

        // define the result serialization. A valid output must be of the following .NET types:
        // System.String, System.Char, System.Int16, System.Int32, System.Int64,
        // System.Float, System.Double, System.Decimal and System.Boolean
        public bool TrySerialize(object value, out object serialized)
        {
            if (value is UPath p)
            {
                serialized = p.FullName;
                return true;
            }
            serialized = null;
            return false;
        }

        public override bool TryDeserialize(object serialized, out object value)
        {
            if (serialized is null)
            {
                value = null;
                return true;
            }

            if (serialized is string s && !Path.EndsInDirectorySeparator(s))
            {
                value = (UPath)s;
                return true;
            }

            value = null;
            return false;
        }

        public override object Serialize(object value)
        {
            return this.TrySerialize(value, out var s) ? s : default;
        }
    }
}

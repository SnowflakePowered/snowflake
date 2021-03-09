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
    public sealed class OSDirectoryPathType
    : ScalarType<DirectoryInfo, StringValueNode>
    {
        /// <summary>
        /// GraphQL Scalar Definition for a <see cref="DirectoryInfo"/> represented as a path string.
        /// </summary>
        public OSDirectoryPathType()
            : base("OSDirectoryPath", BindingBehavior.Implicit)
        {
            Description = "Represents a real, operating system dependent path that points to a directory on the operating system.";
        }

        protected override DirectoryInfo ParseLiteral(StringValueNode literal)
        { 
            var dirPath = new DirectoryInfo(literal.Value);
            return dirPath;
        }

        protected override StringValueNode ParseValue(DirectoryInfo value)
        {
            return new StringValueNode(null, value.FullName, false);
        }

        public override IValueNode ParseResult(object resultValue) => ParseValue(resultValue);

        // define the result serialization. A valid output must be of the following .NET types:
        // System.String, System.Char, System.Int16, System.Int32, System.Int64,
        // System.Float, System.Double, System.Decimal and System.Boolean
        public override bool TrySerialize(object value, out object serialized)
        {
            if (value is DirectoryInfo p)
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

            if (serialized is string s)
            {
                value = new DirectoryInfo(s);
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

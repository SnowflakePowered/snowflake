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
    public sealed class OSFilePathType
    : ScalarType<FileInfo, StringValueNode>
    {
        /// <summary>
        /// GraphQL Scalar Definition for a <see cref="FileInfo"/> represented as a path string.
        /// </summary>
        public OSFilePathType()
            : base("OSFilePath", BindingBehavior.Implicit)
        {
            Description = "Represents a real, operating system dependent path that points to a file on the file system.";
        }

        protected override FileInfo ParseLiteral(StringValueNode literal)
        {
            if (Path.EndsInDirectorySeparator(literal.Value))
                throw new ArgumentException("File paths can not end in the directory separator character.");
            
            var filePath = new FileInfo(literal.Value);
            
            return filePath;
        }

        protected override StringValueNode ParseValue(FileInfo value)
        {
            return new StringValueNode(null, value.FullName, false);
        }

        // define the result serialization. A valid output must be of the following .NET types:
        // System.String, System.Char, System.Int16, System.Int32, System.Int64,
        // System.Float, System.Double, System.Decimal and System.Boolean
        public bool TrySerialize(object value, out object serialized)
        {
            if (value is FileInfo p)
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
                value = new FileInfo(s);
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

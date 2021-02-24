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
    public sealed class OSTaggedFileSystemPathType
    : ScalarType<FileSystemInfo, StringValueNode>
    {
        /// <summary>
        /// GraphQL Scalar Definition for an ambiguous <see cref="FileSystemInfo"/> represented as a tagged path string.
        /// 
        /// Not compatible with <see cref="OSDirectoryPathType"/> or <see cref="OSFilePathType"/>
        /// </summary>
        public OSTaggedFileSystemPathType()
            : base("OSTaggedFileSystemPath", BindingBehavior.Implicit)
        {
            Description = "Represents a real, operating system dependent path that ambiguously points to a file or folder on the file system" +
                "as a tagged path string. The tag is artificial, and is not compatible with OSDirectoryPath or OSFilePath.";
        }

        protected override FileSystemInfo ParseLiteral(StringValueNode literal)
        {
            var taggedParts = literal.Value.Split("|", 2);
            if (taggedParts.Length != 2)
                throw new ArgumentException("FileSystemPath is missing tag! Can not determine type of path.");

            FileSystemInfo fsPath = (taggedParts[0], taggedParts[1]) switch
            {
                ("f", string path) => !Path.EndsInDirectorySeparator(path) ? new FileInfo(path)
                    : throw new ArgumentException("File paths can not end in the directory separator character."),
                ("d", string path) => new DirectoryInfo(path),
                _ => throw new ArgumentException("Unsupported tag for FileSystemPath."),
            };
            return fsPath;
        }

        protected override StringValueNode ParseValue(FileSystemInfo value)
        {
            if (value is FileInfo f) return new StringValueNode(null, $"f|{f.FullName}", false);
            if (value is DirectoryInfo d) return new StringValueNode(null, $"d|{d.FullName}", false);
            throw new ArgumentException("Unsupported FileSystemInfo subtype.");
        }

        public override IValueNode ParseResult(object resultValue) => ParseValue(resultValue);

        // define the result serialization. A valid output must be of the following .NET types:
        // System.String, System.Char, System.Int16, System.Int32, System.Int64,
        // System.Float, System.Double, System.Decimal and System.Boolean
        public override bool TrySerialize(object value, out object serialized)
        {
            if (value is FileInfo f)
            {
                serialized = $"f|{f.FullName}";
                return true;
            }

            if (value is DirectoryInfo d)
            {
                serialized = $"d|{d.FullName}";
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
                var taggedParts = s.Split("|", 2);
                if (taggedParts.Length != 2)
                    throw new ArgumentException("FileSystemPath is missing tag! Can not determine type of path.");

                FileSystemInfo fsPath = (taggedParts[0], taggedParts[1]) switch
                {
                    ("f", string path) => !Path.EndsInDirectorySeparator(path) ? new FileInfo(path)
                        : throw new ArgumentException("File paths can not end in the directory separator character."),
                    ("d", string path) => new DirectoryInfo(path),
                    _ => throw new ArgumentException("Unsupported tag for FileSystemPath."),
                };
                value = fsPath;
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Stone.FileSignatures.Formats
{
    internal static class ByteReaderExtensions
    {
        /// <summary>
        /// Reads a string from the binary reader with a given length.
        /// </summary>
        /// <param name="this">The <see cref="BinaryReader"/> to read from.</param>
        /// <param name="length">The length of the string in bytes</param>
        /// <param name="encoding">The encoding to be used. Defaults to UTF8</param>
        /// <returns></returns>
        public static string ReadString(this BinaryReader @this, int length, Encoding encoding = null)
        {
            return (encoding ?? Encoding.UTF8).GetString(@this.ReadBytes(length));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Snowflake.Filesystem
{
    public static class FileExtensions
    {
        /// <summary>
        /// Opens a text file, reads all the text in the file into a string, and then closes the opened stream.
        /// The default encoding is UTF8.
        /// </summary>
        /// <param name="this">The <see cref="IFile"/> to read.</param>
        /// <returns>A string containing all text in the file.</returns>
        public static string ReadAllText(this IFile @this) => FileExtensions.ReadAllText(@this, Encoding.UTF8);

        /// <summary>
        /// Opens a text file, reads all the text in the file into a string with the specified encoding,
        /// and then closes the opened stream.
        /// </summary>
        /// <param name="this">The <see cref="IFile"/> to read.</param>
        /// <param name="encoding">The encoding applied to the contents of the file.</param>
        /// <returns>A string containing all text in the file.</returns>
        public static string ReadAllText(this IFile @this, Encoding encoding)
        {
            using var fileStream = @this.OpenStream();
            using var stringReader = new StreamReader(fileStream, encoding);
            return stringReader.ReadToEnd();
        }
    }
}

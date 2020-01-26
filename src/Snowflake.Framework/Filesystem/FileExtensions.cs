using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

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
        public static string ReadAllText(this IReadOnlyFile @this) => FileExtensions.ReadAllText(@this, Encoding.UTF8);

        /// <summary>
        /// Opens a text file, reads all the text in the file into a string with the specified encoding,
        /// and then closes the opened stream.
        /// </summary>
        /// <param name="this">The <see cref="IFile"/> to read.</param>
        /// <param name="encoding">The encoding applied to the contents of the file.</param>
        /// <returns>A string containing all text in the file.</returns>
        public static string ReadAllText(this IReadOnlyFile @this, Encoding encoding)
        {
            using var fileStream = @this.OpenReadStream();
            using var stringReader = new StreamReader(fileStream, encoding);
            return stringReader.ReadToEnd();
        }

        /// <summary>
        /// Opens a text file, writes all the text in the file into a string, and then closes the opened stream.
        /// The default encoding is UTF8.
        /// 
        /// If the file already exists, it is overwritten.
        /// </summary>
        /// <param name="this">The <see cref="IFile"/> to read.</param>
        /// <param name="contents">The contents to write to file.</param>
        /// <returns>A string containing all text in the file.</returns>
        public static void WriteAllText(this IFile @this, string contents) => FileExtensions.WriteAllText(@this, contents, Encoding.UTF8);

        /// <summary>
        /// Opens a text file, writes all the text in the file into a string asynchronously, and then closes the opened stream.
        /// The default encoding is UTF8.
        /// 
        /// If the file already exists, it is overwritten.
        /// </summary>
        /// <param name="this">The <see cref="IFile"/> to read.</param>
        /// <param name="contents">The contents to write to file.</param>
        /// <returns>A string containing all text in the file.</returns>
        public static Task WriteAllTextAsync(this IFile @this, string contents) => FileExtensions.WriteAllTextAsync(@this, contents, Encoding.UTF8);

        /// <summary>
        /// Opens a text file, writes all the text in the file into a string with the specified encoding,
        /// and then closes the opened stream. If the file already exists, it is overwritten.
        /// </summary>
        /// <param name="this">The <see cref="IFile"/> to read.</param>
        /// <param name="contents">The contents to write to file.</param>
        /// <param name="encoding">The encoding applied to the contents of the file.</param>
        /// <returns>A string containing all text in the file.</returns>
        public static void WriteAllText(this IFile @this, string contents, Encoding encoding)
        {
            using var fileStream = @this.OpenStream();
            fileStream.SetLength(0);
            using var stringWriter = new StreamWriter(fileStream, encoding);
            stringWriter.Write(contents);
            stringWriter.Flush();
        }

        /// <summary>
        /// Opens a text file, writes all the text in the file into a string with the specified encoding,
        /// and then closes the opened stream. If the file already exists, it is overwritten.
        /// </summary>
        /// <param name="this">The <see cref="IFile"/> to read.</param>
        /// <param name="contents">The contents to write to file.</param>
        /// <param name="encoding">The encoding applied to the contents of the file.</param>
        /// <returns>A string containing all text in the file.</returns>
        public static async Task WriteAllTextAsync(this IFile @this, string contents, Encoding encoding)
        {
            using var fileStream = @this.OpenStream();
            fileStream.SetLength(0);
            using var stringWriter = new StreamWriter(fileStream, encoding);
            await stringWriter.WriteAsync(contents);
            stringWriter.Flush();
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Snowflake.Filesystem
{
    public static class DirectoryExtensions
    {
        /// <summary>
        /// Copies all the files and directories from the source directory recursively into this directory.
        /// </summary>
        /// <param name="this">The destination directory.</param>
        /// <param name="source">The source directory.</param>
        /// <param name="overwrite"></param>
        /// <returns>An enumerable of copied files.</returns>
        public static IAsyncEnumerable<IFile> CopyFromDirectory(this IDirectory @this, IDirectory source) => CopyFromDirectory(@this, source, false);

        /// <summary>
        /// Copies all the files and directories from the source directory recursively into this directory.
        /// </summary>
        /// <param name="this">The destination directory.</param>
        /// <param name="source">The source directory.</param>
        /// <param name="overwrite">Whether or not to overwrite existing files.</param>
        /// <returns>An enumerable of copied files.</returns>
        public static async IAsyncEnumerable<IFile> CopyFromDirectory(this IDirectory @this, IDirectory source, bool overwrite)
        {
            if (@this == source) yield break;

            // Do the parent directory
            foreach (var f in source.EnumerateFiles())
            {
                yield return await @this.CopyFromAsync(f, overwrite);
            }

            var queuedDirs =
                source.EnumerateDirectories()
                .Select(d => (@this, d)).ToList();

            // BFS over all the children.

            Queue<(IDirectory, IDeletableDirectory)> dirsToProcess = new Queue<(IDirectory, IDeletableDirectory)>(queuedDirs);

            while (dirsToProcess.Count > 0)
            {
                var (parent, src) = dirsToProcess.Dequeue();
                var dst = parent.OpenDirectory(src.Name);
                foreach (var f in src.EnumerateFiles())
                {
                    yield return await dst.CopyFromAsync(f, overwrite);
                }

                var children = src.EnumerateDirectories()
                    .Select(d => (dst, d)).ToList();

                foreach (var childDirectory in children)
                {
                    dirsToProcess.Enqueue(childDirectory);
                }
            }
        }
    }
}

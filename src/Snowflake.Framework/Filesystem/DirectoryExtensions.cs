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
        /// <returns>An enumerable of copied files.</returns>
        public static async IAsyncEnumerable<IFile> CopyFromDirectory(this IDirectory @this, IDirectory source)
        {
            // Do the parent directory
            foreach (var f in source.EnumerateFiles())
            {
                yield return await @this.CopyFromAsync(f);
            }

            var queuedDirs =
                source.EnumerateDirectories()
                .Select(d => (@this, d)).ToList();

            // BFS over all the children.

            Queue<(IDirectory, IDirectory)> dirsToProcess = new Queue<(IDirectory, IDirectory)>(queuedDirs);

            while (dirsToProcess.Count > 0)
            {
                var (parent, src) = dirsToProcess.Dequeue();
                var dst = parent.OpenDirectory(src.Name);
                foreach (var f in src.EnumerateFiles())
                {
                    yield return await dst.CopyFromAsync(f);
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

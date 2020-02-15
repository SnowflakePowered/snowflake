using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Filesystem;

namespace Snowflake.Installation.Tasks
{
    /// <summary>
    /// Copies the descendant contents of the provided <see cref="DirectoryInfo"/> into
    /// a desination <see cref="IDirectory"/>.
    /// </summary>
    public sealed class CopyDirectoryContentsTask : AsyncInstallTaskEnumerable<IFile>
    {
        protected override string TaskName => "Copy";

        private TaskResult<DirectoryInfo> Source { get; }
        private IDirectory Destination { get; }
      
        /// <summary>
        /// Describe a directory copy with the provided source and
        /// destination directory.
        /// </summary>
        /// <param name="source">
        /// A <see cref="DirectoryInfo"/> that may or may not be the result of a <see cref="AsyncInstallTask{T}"/> 
        /// where T is <see cref="DirectoryInfo"/>.
        /// </param>
        /// <param name="destinationDirectory">The destination <see cref="IDirectory"/>.</param>
        public CopyDirectoryContentsTask(TaskResult<DirectoryInfo> source, IDirectory destinationDirectory)
        {
            this.Source =  source;
            this.Destination = destinationDirectory;
        }

        protected override async IAsyncEnumerable<IFile> ExecuteOnce()
        {
            // Do the parent directory
            foreach (var f in (await this.Source).EnumerateFiles())
            {
                yield return await this.Destination.CopyFromAsync(f);
            }

            var queuedDirs =
                (await this.Source).EnumerateDirectories()
                .Select(d => (this.Destination, d)).ToList();

            // BFS over all the children.

            Queue<(IDirectory, DirectoryInfo)> dirsToProcess = new Queue<(IDirectory, DirectoryInfo)>(queuedDirs);

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

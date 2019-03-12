using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Filesystem;

namespace Snowflake.Installation.Tasks
{
    public sealed class CopyDirectoryContentsTask : InstallTaskAwaitableEnumerable<IFile?>
    {
        protected override string TaskName => "Copy";

        private DirectoryInfo Source { get; }
        private IDirectory Destination { get; }
      

        public CopyDirectoryContentsTask(DirectoryInfo source, IDirectory destinationDirectory)
        {
            this.Source =  source;
            this.Destination = destinationDirectory;
        }

        protected override async IAsyncEnumerable<IFile?> Execute()
        {

            // Do the parent directory
            foreach (var f in this.Source.EnumerateFiles())
            {
                yield return await this.Destination.CopyFromAsync(f);
            }

            var queuedDirs =
                this.Source.EnumerateDirectories()
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

        private void CreateDirectoryTreeRecursive(IDirectory parent, DirectoryInfo sourceparent)
        {
            foreach (DirectoryInfo d in sourceparent.EnumerateDirectories())
            {
                CreateDirectoryTreeRecursive(parent.OpenDirectory(d.Name), d);
            }
        }
    }
}

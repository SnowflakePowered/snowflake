using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Filesystem;
using Snowflake.Model.Game;
using A = System.Collections.Generic;

namespace Snowflake.Installation.Tasks
{
    /// <summary>
    /// Copies a file into an <see cref="IDirectory"/>
    /// </summary>
    public sealed class CopyFileTask : AsyncInstallTask<IFile?>
    {
        /// <summary>
        /// Describe a file copy with the given source file and the target directory.
        /// The resultant file will have the same file name as the source.
        /// </summary>
        /// <param name="source">The source file.</param>
        /// <param name="destinationDirectory">The target directory.</param>
        public CopyFileTask(TaskResult<FileInfo> source, IDirectory destinationDirectory)
        {
            this.Source = source;
            this.Destination = destinationDirectory;
        }

        private TaskResult<FileInfo> Source { get; }
        private IDirectory Destination { get; }

        protected override string TaskName => "Copy";

        protected override async ValueTask<string> CreateSuccessDescription() 
            => $"Copied {(await this.Source).FullName} to directory {this.Destination.Name}";
        protected override async ValueTask<string> CreateFailureDescription(AggregateException _)
            => $"Failed to copy {(await this.Source).FullName} to directory {this.Destination.Name}";

        protected override async Task<IFile?> ExecuteOnce()
        {
            return await this.Destination.CopyFromAsync(await this.Source);
        }
    }
}

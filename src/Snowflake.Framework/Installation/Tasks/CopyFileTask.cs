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
    public sealed class CopyFileTask : InstallTaskAwaitable<IFile?>
    {
        public CopyFileTask(TaskResult<FileInfo> source, IDirectory destinationDirectory)
        {
            this.Source = source;
            this.Destination = destinationDirectory;
        }
        private TaskResult<FileInfo> Source { get; }
        private IDirectory Destination { get; }

        protected override string TaskName => "Copy";

        protected override async Task<IFile?> ExecuteOnce()
        {
            return await this.Destination.CopyFromAsync(await this.Source);
        }
    }
}

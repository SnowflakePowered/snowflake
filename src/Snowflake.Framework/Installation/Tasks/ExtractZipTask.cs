using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Snowflake.Filesystem;

namespace Snowflake.Installation.Tasks
{
    public sealed class ExtractZipTask : InstallTaskAwaitableEnumerable<IFile?>
    {
        public ExtractZipTask(TaskResult<FileInfo> fileInfo, IDirectory destination)
        {
            this.FileInfo = fileInfo;
            this.Destination = destination;
        }

        public TaskResult<FileInfo> FileInfo { get; }
        public IDirectory Destination { get; }

        protected override string TaskName => "Zip Extract";

        protected override async IAsyncEnumerable<IFile?> ExecuteOnce()
        {
            using FileStream zipToOpen = (await this.FileInfo).OpenRead();
            using ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read);

            foreach (var entries in archive.Entries)
            {
                // todo: this should recursively create folders instead of dumping it all in front...
                var file = this.Destination.OpenFile(entries.Name);
                using var fileStream = file.OpenStream();
                using var zipStream = entries.Open();
                await zipStream.CopyToAsync(fileStream);
                yield return file;
            }
        }
    }
}

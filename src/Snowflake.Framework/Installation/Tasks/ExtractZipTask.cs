using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Snowflake.Filesystem;

namespace Snowflake.Installation.Tasks
{
    /// <summary>
    /// Extracts a ZIP file into a target directory.
    /// </summary>
    public sealed class ExtractZipTask : AsyncInstallTaskEnumerable<IFile?>
    {
        /// <summary>
        /// Describes an extraction of a ZIP file into a target directory.
        /// </summary>
        /// <param name="fileInfo">The ZIP file source.</param>
        /// <param name="destination">The destination directory.</param>
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
                string directoryName = Path.GetDirectoryName(entries.FullName);
                // create the destination subdirectory.
                var extractDest = this.Destination.OpenDirectory(directoryName);
                var file = extractDest.OpenFile(entries.Name);
                if (String.IsNullOrWhiteSpace(entries.Name)) continue;
                using (var zipStream = entries.Open())
                using (var fileStream = file.OpenStream())
                {
                    await zipStream.CopyToAsync(fileStream).ConfigureAwait(false);
                    await fileStream.FlushAsync().ConfigureAwait(false);
                    yield return file;
                }
            }
        }
    }
}

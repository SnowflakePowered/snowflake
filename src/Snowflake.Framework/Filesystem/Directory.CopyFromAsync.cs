using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Filesystem
{
    internal sealed partial class Directory
    {
        public Task<IFile> CopyFromAsync(IReadOnlyFile source, CancellationToken cancellation = default)
       => this.CopyFromAsync(source, false, cancellation);

        public Task<IFile> CopyFromAsync(IReadOnlyFile source, bool overwrite, CancellationToken cancellation = default)
            => this.CopyFromAsync(source, source.Name, overwrite, cancellation);

        public Task<IFile> CopyFromAsync(IReadOnlyFile source, string targetName, CancellationToken cancellation = default)
            => this.CopyFromAsync(source, targetName, false, cancellation);

        public Task<IFile> CopyFromAsync(FileInfo source, CancellationToken cancellation = default)
            => this.CopyFromAsync(source, false, cancellation);

        public Task<IFile> CopyFromAsync(FileInfo source, bool overwrite, CancellationToken cancellation = default)
            => this.CopyFromAsync(source, source.Name, overwrite, cancellation);

        public async Task<IFile> CopyFromAsync(IReadOnlyFile source, string targetName, bool overwrite, CancellationToken cancellation = default)
        {
            if (this.ContainsFile(Path.GetFileName(targetName)) && !overwrite) throw new IOException($"{source.Name} already exists in the target directory");
#pragma warning disable CS0618 // Type or member is obsolete
            return await this.CopyFromAsync(source.UnsafeGetPath(), targetName, overwrite, cancellation);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public Task<IFile> CopyFromAsync(FileInfo source, string targetName, CancellationToken cancellation = default)
            => this.CopyFromAsync(source, targetName, false, cancellation);

        public async Task<IFile> CopyFromAsync(FileInfo source, string targetName, bool overwrite, CancellationToken cancellation = default)
        {
            this.CheckDeleted();
            if (!source.ContentExists()) throw new FileNotFoundException($"{source.FullName} could not be found.");
            string? fileName = Path.GetFileName(targetName);
            if (!Directory.IsValidFileName(fileName)) throw new DirectoryNotFoundException($"The filename {targetName} is invalid.");

            // Preserve GUID
            if (!this.FileGuidProvider.TryGetGuid(source, out Guid existingGuid))
                existingGuid = Guid.NewGuid();

            var file = this.OpenFile(fileName, existingGuid);
            if (file.Created && !overwrite) throw new IOException($"{source.Name} already exists in the target directory.");

            using (var newStream = file.OpenStream(FileMode.Create, FileAccess.ReadWrite))
            using (var sourceStream = source.OpenRead())
            {
                await sourceStream.CopyToAsync(newStream, cancellation);
            }

            return file;
        }
    }
}

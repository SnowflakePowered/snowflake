using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Filesystem
{
    internal class DisposableDirectory
        : IDisposableDirectory
    {
        private bool disposedValue;

        public DisposableDirectory(Directory baseDirectory)
        {
            this.Base = baseDirectory;
        }

        public Directory Base { get; }

        public string Name => this.Base.Name;

        public bool ContainsDirectory(string directory) => this.Base.ContainsDirectory(directory);

        public bool ContainsFile(string file) => this.Base.ContainsDirectory(file);

        public IFile CopyFrom(System.IO.FileInfo source) => this.Base.CopyFrom(source);

        public IFile CopyFrom(System.IO.FileInfo source, bool overwrite) => this.Base.CopyFrom(source, overwrite);

        public IFile CopyFrom(IReadOnlyFile source) => this.Base.CopyFrom(source);

        public IFile CopyFrom(IReadOnlyFile source, bool overwrite) => this.Base.CopyFrom(source);

        public Task<IFile> CopyFromAsync(System.IO.FileInfo source, CancellationToken cancellation = default)
                => this.Base.CopyFromAsync(source, cancellation);

        public Task<IFile> CopyFromAsync(System.IO.FileInfo source, bool overwrite, CancellationToken cancellation = default)
            => this.Base.CopyFromAsync(source, overwrite, cancellation);

        public Task<IFile> CopyFromAsync(IReadOnlyFile source, CancellationToken cancellation = default)
            => this.Base.CopyFromAsync(source, cancellation);

        public Task<IFile> CopyFromAsync(IReadOnlyFile source, bool overwrite, CancellationToken cancellation = default)
            => this.Base.CopyFromAsync(source, overwrite, cancellation);

        public IEnumerable<IFile> EnumerateFiles() => this.Base.EnumerateFiles();

        public IEnumerable<IFile> EnumerateFilesRecursive() => this.Base.EnumerateFilesRecursive();

        public IFile OpenFile(string file) => this.Base.OpenFile(file);

        public System.IO.DirectoryInfo UnsafeGetPath() => this.Base.UnsafeGetPath();

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && this.Base.IsDeleted)
                {
                    disposedValue = true;
                } 
                else if (disposing)
                {
                    try
                    {
                        this.Base.Delete();
                    } 
                    catch 
                    {
                        disposedValue = true;
                    }
                }
                disposedValue = true;
            }
        }
        
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public IFile LinkFrom(System.IO.FileInfo source) => this.Base.LinkFrom(source);
        public IFile LinkFrom(System.IO.FileInfo source, bool overwrite) => this.Base.LinkFrom(source, overwrite);
        IDeletableDirectory IDirectoryOpeningDirectoryBase<IDeletableDirectory>.OpenDirectory(string name) => this.Base.OpenDirectory(name);
        IReadOnlyDirectory IReopenableDirectoryBase<IReadOnlyDirectory>.ReopenAs() => this.Base.AsReadOnly();
        IMoveFromableDirectory IReopenableDirectoryBase<IMoveFromableDirectory>.ReopenAs() => this.Base.AsIndelible().AsMoveFromable();
        public IEnumerable<IDeletableDirectory> EnumerateDirectories() => this.Base.EnumerateDirectories();

        public IFile CopyFrom(System.IO.FileInfo source, string targetName)
            => this.Base.CopyFrom(source, targetName);
        public IFile CopyFrom(System.IO.FileInfo source, string targetName, bool overwrite)
            => this.Base.CopyFrom(source, targetName, overwrite);

        public Task<IFile> CopyFromAsync(System.IO.FileInfo source, string targetName, CancellationToken cancellation = default)
            => this.Base.CopyFromAsync(source, targetName, cancellation);

        public Task<IFile> CopyFromAsync(System.IO.FileInfo source, string targetName, bool overwrite, CancellationToken cancellation = default)
            => this.Base.CopyFromAsync(source, targetName, overwrite);

        public IFile CopyFrom(IReadOnlyFile source, string targetName)
            => this.Base.CopyFrom(source, targetName);

        public IFile CopyFrom(IReadOnlyFile source, string targetName, bool overwrite)
            => this.Base.CopyFrom(source, targetName, overwrite);

        public Task<IFile> CopyFromAsync(IReadOnlyFile source, string targetName, CancellationToken cancellation = default)
            => this.Base.CopyFromAsync(source, targetName, cancellation);

        public Task<IFile> CopyFromAsync(IReadOnlyFile source, string targetName, bool overwrite, CancellationToken cancellation = default)
            => this.Base.CopyFromAsync(source, targetName, overwrite, cancellation);
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using IO = System.IO;
using System.Text;
using System.Linq;
using Zio;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Model.FileSystem
{
    internal class ManifestedDirectory : IManifestedDirectory
    {
        Directory BackingDirectory { get; }

        internal ManifestedDirectory(string name, IFileSystem parentFs)
        {
            // todo: create manifest.
            this.BackingDirectory = new Directory(name, parentFs);
        }

        private string GuessMimetype(FileInfo f)
        {
            return "application/octet-stream";
        }

        public string Name => this.BackingDirectory.Name;

        public bool HasManifest => true;

        public bool ContainsDirectory(string directory) => this.BackingDirectory.ContainsDirectory(directory);

        public bool ContainsFile(string file) => this.BackingDirectory.ContainsFile(file);

        public bool ContainsManifestedFile(string file)
        {
            throw new NotImplementedException();
        }

        internal void DeleteGuidEntry(Guid fileGuid)
        {
            throw new NotImplementedException();
        }

        internal void RenameGuidEntry(Guid fileGuid, string newName)
        {
            throw new NotImplementedException();
        }

        internal Guid GetGuidEntry(string fileName)
        {
            throw new NotImplementedException();
        }

        public bool ContainsManifestedFile(Guid fileguid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IManifestedFile> EnumerateManifestedFiles()
        {
            throw new NotImplementedException();
        }

        public IManifestedFile OpenFile(string file)
        {
            if (file.ToLowerInvariant() == ".manifest")
            {
                throw new UnauthorizedAccessException("Not allowed to access the manifest file.");
            }

            throw new NotImplementedException();
        }

        public IManifestedFile OpenFile(Guid guid)
        {
            throw new NotImplementedException();
        }

        public IManifestedFile SetMimeType(Guid file, string mimetype)
        {
            throw new NotImplementedException();
        }

        public IManifestedFile? CopyFrom(FileInfo source)
            => this.CopyFrom(source, false);
        public IManifestedFile? CopyFrom(FileInfo source, bool overwrite)
            => this.CopyFrom(source, this.GuessMimetype(source), overwrite);

        public IManifestedFile? CopyFrom(FileInfo source, string mimetype)
            => this.CopyFrom(source, mimetype, false);

        public IManifestedFile? CopyFrom(FileInfo source, string mimetype, bool overwrite)
        {
            var file =  this.BackingDirectory.CopyFrom(source, overwrite);
            if (file == null) return null;

            var guid = this.GetGuidEntry(file.Name);
            this.SetMimeType(guid, mimetype);
            return new ManifestedFile(this, ((File)file).RawInfo, guid, mimetype);
        }

        public Task<IManifestedFile?> CopyFromAsync(FileInfo source)
            => this.CopyFromAsync(source, false);

        public Task<IManifestedFile?> CopyFromAsync(FileInfo source, bool overwrite)
            => this.CopyFromAsync(source, overwrite, CancellationToken.None);

        public Task<IManifestedFile?> CopyFromAsync(FileInfo source, bool overwrite, CancellationToken cancel)
            => this.CopyFromAsync(source, this.GuessMimetype(source), overwrite, cancel);
        public Task<IManifestedFile?> CopyFromAsync(FileInfo source, string mimetype)
            => this.CopyFromAsync(source, mimetype);

        public Task<IManifestedFile?> CopyFromAsync(FileInfo source, string mimetype, bool overwrite)
            => this.CopyFromAsync(source, mimetype, overwrite, CancellationToken.None);

        public async Task<IManifestedFile?> CopyFromAsync(FileInfo source, string mimetype, bool overwrite, CancellationToken cancel)
        {
            var file = await this.BackingDirectory.CopyFromAsync(source, overwrite, cancel);
            if (file == null) return null;

            var guid = this.GetGuidEntry(file.Name);
            this.SetMimeType(guid, mimetype);
            return new ManifestedFile(this, ((File)file).RawInfo, guid, mimetype);
        }

        public IManifestedFile? CopyFrom(IFile source)
            => this.CopyFrom(source, false);

        public IManifestedFile? CopyFrom(IFile source, bool overwrite)
#pragma warning disable CS0618 // Type or member is obsolete
            => this.CopyFrom(source, this.GuessMimetype(source.GetFilePath()), overwrite);
#pragma warning restore CS0618 // Type or member is obsolete

        public IManifestedFile? CopyFrom(IFile source, string mimetype)
            => this.CopyFrom(source, mimetype);

        public IManifestedFile? CopyFrom(IFile source, string mimetype, bool overwrite)
#pragma warning disable CS0618 // Type or member is obsolete
            => this.CopyFrom(source.GetFilePath(), mimetype, overwrite);
#pragma warning restore CS0618 // Type or member is obsolete

        public Task<IManifestedFile?> CopyFromAsync(IFile source)
            => this.CopyFromAsync(source, false);

        public Task<IManifestedFile?> CopyFromAsync(IFile source, bool overwrite)
            => this.CopyFromAsync(source, overwrite, CancellationToken.None);

        public Task<IManifestedFile?> CopyFromAsync(IFile source, bool overwrite, CancellationToken cancel)
#pragma warning disable CS0618 // Type or member is obsolete
            => this.CopyFromAsync(source, this.GuessMimetype(source.GetFilePath()), overwrite, cancel);
#pragma warning restore CS0618 // Type or member is obsolete

        public Task<IManifestedFile?> CopyFromAsync(IFile source, string mimetype)
            => this.CopyFromAsync(source, mimetype);

        public Task<IManifestedFile?> CopyFromAsync(IFile source, string mimetype, bool overwrite)
            => this.CopyFromAsync(source, mimetype, overwrite, CancellationToken.None);
        public Task<IManifestedFile?> CopyFromAsync(IFile source, string mimetype, bool overwrite, CancellationToken cancel)
#pragma warning disable CS0618 // Type or member is obsolete
            => this.CopyFromAsync(source.GetFilePath(), mimetype, overwrite, cancel);
#pragma warning restore CS0618 // Type or member is obsolete

        public IDirectory OpenDirectory(string name) => this.BackingDirectory.OpenDirectory(name);

        public IManifestedDirectory OpenManifestedDirectory(string name) 
            => this.BackingDirectory.OpenManifestedDirectory(name);

        IFile IDirectory.OpenFile(string file) => this.OpenFile(file);

        IFile? IDirectory.CopyFrom(FileInfo source) => this.CopyFrom(source);

        IFile? IDirectory.CopyFrom(FileInfo source, bool overwrite) => this.CopyFrom(source, overwrite);

        async Task<IFile?> IDirectory.CopyFromAsync(FileInfo source) => await this.CopyFromAsync(source);

        async Task<IFile?> IDirectory.CopyFromAsync(FileInfo source, bool overwrite)
            => await this.CopyFromAsync(source, overwrite);

        async Task<IFile?> IDirectory.CopyFromAsync(FileInfo source, bool overwrite, CancellationToken cancellation)
            => await this.CopyFromAsync(source, overwrite, cancellation);

        IFile? IDirectory.MoveFrom(IFile source) => this.MoveFrom(source);

        IFile? IDirectory.MoveFrom(IFile source, bool overwrite) => this.MoveFrom(source, overwrite);
        IFile? IDirectory.CopyFrom(IFile source) => this.CopyFrom(source);

        IFile? IDirectory.CopyFrom(IFile source, bool overwrite) => this.CopyFrom(source, overwrite);

        async Task<IFile?> IDirectory.CopyFromAsync(IFile source) => await this.CopyFromAsync(source);

        async Task<IFile?> IDirectory.CopyFromAsync(IFile source, bool overwrite) 
            => await this.CopyFromAsync(source, overwrite);

        async Task<IFile?> IDirectory.CopyFromAsync(IFile source, bool overwrite, CancellationToken cancellation)
           => await this.CopyFromAsync(source, overwrite, cancellation);
        public IEnumerable<IDirectory> EnumerateDirectories()
            => this.BackingDirectory.EnumerateDirectories();

        public IEnumerable<IFile> EnumerateFiles()
            => this.BackingDirectory.EnumerateFiles();

        public DirectoryInfo GetPath()
            => this.BackingDirectory.GetPath();


        public IManifestedFile? MoveFrom(IFile source)
            => this.MoveFrom(source, false);

        public IManifestedFile? MoveFrom(IFile source, bool overwrite)
#pragma warning disable CS0618 // Type or member is obsolete
            => this.MoveFrom(source, overwrite, this.GuessMimetype(source.GetFilePath()));
#pragma warning restore CS0618 // Type or member is obsolete

        public IManifestedFile? MoveFrom(IFile source, bool overwrite, string mimetype)
        {
            var file = this.BackingDirectory.MoveFrom(source, overwrite);
            if (file == null) return null;

            var guid = this.GetGuidEntry(file.Name);
            this.SetMimeType(guid, mimetype);
            return new ManifestedFile(this, ((File)file).RawInfo, guid, mimetype);
        }

        public IManifestedFile? MoveFrom(IManifestedFile source)
            => this.MoveFrom(source, false);

        public IManifestedFile? MoveFrom(IManifestedFile source, bool overwrite)
            => this.MoveFrom(source, overwrite, source.MimeType);

        public IManifestedDirectory AsManifestedDirectory()
        {
            return this;
        }
    }
}

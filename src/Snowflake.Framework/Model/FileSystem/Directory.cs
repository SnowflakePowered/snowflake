using System;
using System.Collections.Generic;
using System.IO;
using IO = System.IO;
using System.Text;
using System.Linq;
using Zio;
using System.Threading.Tasks;
using System.Threading;

namespace Snowflake.Model.FileSystem
{
    internal sealed class Directory : IDirectory
    {
        internal Directory(string name, IFileSystem parentFs)
        {
            this.ParentFileSystem = parentFs;
            this.FileSystem = parentFs.GetOrCreateSubFileSystem((UPath)"/" / name);
            this.Name = name;
        }

        private IFileSystem ParentFileSystem { get; }
        private IFileSystem FileSystem { get; }

        public string Name { get; }

        public bool HasManifest => this.FileSystem.FileExists("/.manifest");

        public bool ContainsDirectory(string directory)
        {
            return this.FileSystem.DirectoryExists((UPath)"/" / directory);
        }

        public bool ContainsFile(string file)
        {
            return this.FileSystem.FileExists((UPath)"/" / file);
        }

        public IDirectory OpenDirectory(string name)
        {
            return new Directory(name, 
               this.FileSystem.GetOrCreateSubFileSystem((UPath)"/" / new UPath(name).GetName()));
        }

        public IManifestedDirectory OpenManifestedDirectory(string name)
        {
            return new ManifestedDirectory(name,
             this.FileSystem.GetOrCreateSubFileSystem((UPath)"/" / new UPath(name).GetName()));
        }

        public IEnumerable<IDirectory> EnumerateDirectories()
        {
            return this.FileSystem.EnumerateDirectories("/")
                .Select(d => this.OpenDirectory(d.GetName()));
        }

        public IEnumerable<IFile> EnumerateFiles()
        {
            return this.FileSystem.EnumerateFiles("/")
                    .Select(f => this.OpenFile(f));
        }

        private IFile OpenFile(UPath file)
        {
           return new File(this, new FileEntry(this.FileSystem, file));
        }

      
        public DirectoryInfo GetPath()
        {
            return new DirectoryInfo(this.FileSystem.ConvertPathToInternal("/"));
        }

        public IFile OpenFile(string file)
        {
            return this.OpenFile((UPath)"/" / Path.GetFileName(file));
        }

        public IFile? CopyFrom(FileInfo source) => this.CopyFrom(source, false);

        public IFile? CopyFrom(FileInfo source, bool overwrite)
        {
            if (!source.Exists) return null;
            string fileName = Path.GetFileName(source.Name);
            source.CopyTo(this.FileSystem.ConvertPathToInternal($"/{fileName}"));
            return this.OpenFile(fileName);
        }

        public Task<IFile?> CopyFromAsync(FileInfo source, bool overwrite)
            => this.CopyFromAsync(source, overwrite, CancellationToken.None);

        public Task<IFile?> CopyFromAsync(FileInfo source) => this.CopyFromAsync(source, false);

        public async Task<IFile?> CopyFromAsync(FileInfo source, bool overwrite, CancellationToken cancellation)
        {
            if (!source.Exists) return null;
            string fileName = Path.GetFileName(source.Name);
            var file = this.OpenFile(fileName);
            if (file.Created && !overwrite) return null;
            using (var newStream = file.OpenStream())
            using (var sourceStream = source.OpenRead())
            {
                await sourceStream.CopyToAsync(newStream, cancellation);
            }
            return file;
        }

        public IFile? CopyFrom(IFile source, bool overwrite)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            return this.CopyFrom(source.GetFilePath(), overwrite);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public Task<IFile?> CopyFromAsync(IFile source) => this.CopyFromAsync(source, false);

        public Task<IFile?> CopyFromAsync(IFile source, bool overwrite) =>
            this.CopyFromAsync(source, overwrite, CancellationToken.None);

        public Task<IFile?> CopyFromAsync(IFile source, bool overwrite, CancellationToken cancellation)
#pragma warning disable CS0618 // Type or member is obsolete
            => this.CopyFromAsync(source.GetFilePath(), overwrite, cancellation);
#pragma warning restore CS0618 // Type or member is obsolete

        public IFile? CopyFrom(IFile source) => this.CopyFrom(source, false);

        public IFile? MoveFrom(IFile source) => this.MoveFrom(source, false);

        public IFile? MoveFrom(IFile source, bool overwrite)
        {
            if (!source.Created) return null;
            var file = this.OpenFile(source.Name);
            // unsafe usage here as optimization.
#pragma warning disable CS0618 // Type or member is obsolete
            source.GetFilePath().MoveTo(file.GetFilePath().ToString());
#pragma warning restore CS0618 // Type or member is obsolete
            source.Delete();
            return file;
        }

        public IManifestedDirectory? AsManifestedDirectory()
        {
            if (!this.HasManifest) return null;
            return new ManifestedDirectory(this.Name, this.ParentFileSystem);
        }

        public IEnumerable<IManifestedDirectory> EnumerateManifestedDirectories()
        {
            return this.EnumerateDirectories()
                .Where(d => d.HasManifest).Select(d => d.AsManifestedDirectory()!);
        }
    }
}

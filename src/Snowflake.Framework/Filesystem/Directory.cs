using System;
using System.Collections.Generic;
using System.IO;
using IO = System.IO;
using System.Text;
using System.Linq;
using Zio;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using Snowflake.Persistence;
using Dapper;

namespace Snowflake.Filesystem
{
    internal sealed class Directory : IDirectory
    {
        private SqliteDatabase Manifest { get; }

        internal Directory(string name, IFileSystem rootFs, DirectoryEntry parentDirectory)
        {
            this.RootFileSystem = rootFs;
            this.ThisDirectory = parentDirectory.CreateSubdirectory(name);
            this.Name = name;

            this.Manifest =
                new SqliteDatabase(this.RootFileSystem.ConvertPathToInternal(this.ThisDirectory.Path / ".manifest"));
            this.Manifest.CreateTable("directory_manifest", "uuid UUID", "filename TEXT PRIMARY KEY");
        }

        internal Directory(IFileSystem rootFs)
        {
            this.RootFileSystem = rootFs;
            this.ThisDirectory = rootFs.GetDirectoryEntry("/");
            this.Name = "#FSROOT";

            this.Manifest =
                new SqliteDatabase(this.RootFileSystem.ConvertPathToInternal(this.ThisDirectory.Path / ".manifest"));
            this.Manifest.CreateTable("directory_manifest", "uuid UUID", "filename TEXT PRIMARY KEY");
        }

        private void AddGuid(string file, Guid guid)
        {
            this.Manifest.Execute(connection =>
            {
                connection.Execute(
                    @"INSERT OR REPLACE INTO directory_manifest (uuid, filename) VALUES (@guid, @file)",
                    new {guid, file});
            });
        }

        internal void RemoveGuid(string file)
        {
            this.Manifest.Execute(connection =>
            {
                connection.Execute(@"DELETE FROM directory_manifest WHERE filename = @file", new {file});
            });
        }

        internal Guid GetGuid(string file)
        {
            return this.Manifest.Query(conn =>
            {
                var bytes = conn.Query<byte[]>(@"SELECT uuid FROM directory_manifest WHERE filename = @file",
                    new {file});
                if (bytes.Count() == 0)
                {
                    conn.Execute(@"INSERT OR REPLACE INTO directory_manifest (uuid, filename) VALUES (@guid, @file)",
                        new {guid = Guid.NewGuid(), file});
                    bytes = conn.Query<byte[]>(@"SELECT uuid FROM directory_manifest WHERE filename = @file",
                        new {file});
                    return new Guid(bytes.First());
                }

                return new Guid(bytes.First());
            });
        }

        internal IFileSystem RootFileSystem { get; }

        internal DirectoryEntry ThisDirectory { get; }
        //internal IFileSystem FileSystem { get; }

        public string Name { get; }

        public bool ContainsDirectory(string directory)
        {
            return this.RootFileSystem.DirectoryExists(this.ThisDirectory.Path / directory);
        }

        public bool ContainsFile(string file)
        {
            return this.RootFileSystem.FileExists(this.ThisDirectory.Path / file);
        }

        public IDirectory OpenDirectory(string name)
        {
            return new Directory(name,
                this.RootFileSystem,
                this.ThisDirectory);
        }

        public IEnumerable<IDirectory> EnumerateDirectories()
        {
            return this.ThisDirectory.EnumerateDirectories()
                .Select(d => this.OpenDirectory(d.Name));
        }

        public IEnumerable<IFile> EnumerateFiles()
        {
            return this.ThisDirectory.EnumerateFiles()
                .Where(f => f.Name != ".manifest")
                .Select(f => this.OpenFile(f.Name));
        }

        private IFile OpenFile(UPath file)
        {
            if (file.GetName() == ".manifest") throw new UnauthorizedAccessException("Unable to open manifest file.");
            return new File(this, new FileEntry(this.RootFileSystem, file),
                this.GetGuid(file.GetName()));
        }

        public DirectoryInfo GetPath()
        {
            return new DirectoryInfo(this.RootFileSystem.ConvertPathToInternal(this.ThisDirectory.Path));
        }

        public IFile OpenFile(string file)
        {
            return this.OpenFile(this.ThisDirectory.Path / Path.GetFileName(file));
        }

        public IFile? CopyFrom(FileInfo source) => this.CopyFrom(source, false);

        public IFile? CopyFrom(FileInfo source, bool overwrite)
        {
            if (!source.Exists) return null;
            string fileName = Path.GetFileName(source.Name);
            source.CopyTo(this.RootFileSystem.ConvertPathToInternal(this.ThisDirectory.Path / fileName));
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
            if (this.ContainsFile(source.Name) && !overwrite) return null;
            this.AddGuid(source.Name, source.FileGuid);
#pragma warning disable CS0618 // Type or member is obsolete
            return this.CopyFrom(source.GetFilePath(), overwrite);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public Task<IFile?> CopyFromAsync(IFile source) => this.CopyFromAsync(source, false);

        public Task<IFile?> CopyFromAsync(IFile source, bool overwrite) =>
            this.CopyFromAsync(source, overwrite, CancellationToken.None);

        public async Task<IFile?> CopyFromAsync(IFile source, bool overwrite, CancellationToken cancellation)
        {
            if (this.ContainsFile(source.Name) && !overwrite) return null;
            this.AddGuid(source.Name, source.FileGuid);
#pragma warning disable CS0618 // Type or member is obsolete
            return await this.CopyFromAsync(source.GetFilePath(), overwrite, cancellation);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public IFile? CopyFrom(IFile source) => this.CopyFrom(source, false);

        public IFile? MoveFrom(IFile source) => this.MoveFrom(source, false);

        public IFile? MoveFrom(IFile source, bool overwrite)
        {
            if (!source.Created) return null;
            if (this.ContainsFile(source.Name) && !overwrite) return null;
            this.AddGuid(source.Name, source.FileGuid);
            var file = this.OpenFile(source.Name);
            // unsafe usage here as optimization.
#pragma warning disable CS0618 // Type or member is obsolete
            source.GetFilePath().MoveTo(file.GetFilePath().ToString());
#pragma warning restore CS0618 // Type or member is obsolete
            source.Delete();
            return file;
        }

        public IEnumerable<IFile> EnumerateFilesRecursive()
        {
            return this.EnumerateFiles()
                .Concat(this.EnumerateDirectories().SelectMany(d => d.EnumerateFilesRecursive()));
        }
    }
}

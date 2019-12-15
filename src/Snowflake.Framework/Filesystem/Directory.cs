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
    internal sealed class Directory : IDirectory, IReadOnlyDirectory
    {
        private SqliteDatabase Manifest { get; }

        internal Directory(string name, IFileSystem rootFs, DirectoryEntry parentDirectory)
        {
            this.RootFileSystem = rootFs;
            this.ThisDirectory = parentDirectory.CreateSubdirectory(name);
            this.Name = name;

            this.Manifest =
                new SqliteDatabase(this.RootFileSystem.ConvertPathToInternal(this.ThisDirectory.Path / ".manifest"));
            this.Manifest.CreateTable("directory_manifest", "uuid TEXT", "filename TEXT PRIMARY KEY");
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

        internal void AddGuid(string file, Guid guid)
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
                var bytes = conn.Query<string>(@"SELECT uuid FROM directory_manifest WHERE filename = @file",
                    new {file});
                if (bytes.Count() == 0)
                {
                    conn.Execute(@"INSERT OR REPLACE INTO directory_manifest (uuid, filename) VALUES (@guid, @file)",
                        new {guid = Guid.NewGuid(), file});
                    bytes = conn.Query<string>(@"SELECT uuid FROM directory_manifest WHERE filename = @file",
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
            var directoryStrings = ((UPath)name).Split();

            Directory currentDirectory = this;
            foreach (string directoryString in directoryStrings)
            {
                currentDirectory = new Directory(directoryString, this.RootFileSystem, currentDirectory.ThisDirectory);
            }
            return currentDirectory;
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

        public DirectoryInfo UnsafeGetPath()
        {
            return new DirectoryInfo(this.RootFileSystem.ConvertPathToInternal(this.ThisDirectory.Path));
        }

        public IFile OpenFile(string file)
        {
            return this.OpenFile(this.ThisDirectory.Path / Path.GetFileName(file));
        }

        public IFile CopyFrom(FileInfo source) => this.CopyFrom(source, false);

        public IFile CopyFrom(FileInfo source, bool overwrite)
        {
            if (!source.Exists) throw new FileNotFoundException($"{source.FullName} could not be found.");
            string? fileName = Path.GetFileName(source.Name);

            if (fileName == null) throw new ArgumentException($"Could not get file name for path {source.Name}.");

            source.CopyTo(this.RootFileSystem.ConvertPathToInternal(this.ThisDirectory.Path / fileName), overwrite);
            return this.OpenFile(fileName);
        }

        public Task<IFile> CopyFromAsync(FileInfo source, CancellationToken cancellation = default) 
            => this.CopyFromAsync(source, false, cancellation);

        public async Task<IFile> CopyFromAsync(FileInfo source, bool overwrite, CancellationToken cancellation = default)
        {
            if (!source.Exists) throw new FileNotFoundException($"{source.FullName} could not be found.");
            string? fileName = Path.GetFileName(source.Name);
            if (fileName == null) throw new ArgumentException($"Cannot get file name for path {source.Name}");
            var file = this.OpenFile(fileName);
            if (file.Created && !overwrite) throw new IOException($"{source.Name} already exists in the target directory.");
            using (var newStream = file.OpenStream())
            using (var sourceStream = source.OpenRead())
            {
                await sourceStream.CopyToAsync(newStream, cancellation);
            }

            return file;
        }

        public IFile CopyFrom(IReadOnlyFile source, bool overwrite)
        {
            if (this.ContainsFile(source.Name) && !overwrite) throw new IOException($"{source.Name} already exists in the target directory.");
            this.AddGuid(source.Name, source.FileGuid);
#pragma warning disable CS0618 // Type or member is obsolete
            return this.CopyFrom(source.UnsafeGetFilePath(), overwrite);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public Task<IFile> CopyFromAsync(IReadOnlyFile source, CancellationToken cancellation = default) 
            => this.CopyFromAsync(source, false, cancellation);

        public async Task<IFile> CopyFromAsync(IReadOnlyFile source, bool overwrite, CancellationToken cancellation = default)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            if (this.ContainsFile(source.Name) && !overwrite) throw new IOException($"{source.Name} already exists in the target directory");
            this.AddGuid(source.Name, source.FileGuid);
            return await this.CopyFromAsync(source.UnsafeGetFilePath(), overwrite, cancellation);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public IFile CopyFrom(IReadOnlyFile source) => this.CopyFrom(source, false);

        public IFile MoveFrom(IFile source) => this.MoveFrom(source, false);

        public IFile MoveFrom(IFile source, bool overwrite)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            if (!source.Created) throw new FileNotFoundException($"{source.UnsafeGetFilePath().FullName} could not be found.");
            if (this.ContainsFile(source.Name) && !overwrite) throw new IOException($"{source.Name} already exists in the target directory");
            this.AddGuid(source.Name, source.FileGuid);
            var file = this.OpenFile(source.Name);
            // unsafe usage here as optimization.
            source.UnsafeGetFilePath().MoveTo(file.UnsafeGetFilePath().ToString(), overwrite);
#pragma warning restore CS0618 // Type or member is obsolete
            source.Delete();
            return file;
        }

        public IEnumerable<IFile> EnumerateFilesRecursive()
        {
            // Do the parent directory
            foreach (var f in this.EnumerateFiles())
            {
                yield return f;
            }

            var queuedDirs =
                this.EnumerateDirectories();

            // BFS over all the children.

            Queue<IDirectory> dirsToProcess = new Queue<IDirectory>(queuedDirs);

            while (dirsToProcess.Count > 0)
            {
                var dir = dirsToProcess.Dequeue();
                foreach (var f in dir.EnumerateFiles())
                {
                    yield return f;
                }

                foreach (var childDirectory in dir.EnumerateDirectories())
                {
                    dirsToProcess.Enqueue(childDirectory);
                }
            }
        }

        IReadOnlyDirectory IReadOnlyDirectory.OpenDirectory(string name)
        {
            if (this.ContainsDirectory(name)) return (Directory)this.OpenDirectory(name);
            throw new DirectoryNotFoundException($"Directory {name} does not exist within {this.Name}.");
        }

        IReadOnlyDirectory IReadOnlyDirectory.OpenDirectory(string name, bool createIfNotExists)
        {
            if (createIfNotExists) return this.OpenDirectory(name).AsReadOnly();
            return (this as IReadOnlyDirectory).OpenDirectory(name);
        }

        IReadOnlyFile IReadOnlyDirectory.OpenFile(string file)
        {
            if (this.ContainsFile(file)) return this.OpenFile(file).AsReadOnly();
            throw new FileNotFoundException($"File {file} does not exist within the directory {this.Name}.");
        }

        IReadOnlyFile IReadOnlyDirectory.OpenFile(string file, bool createIfNotExists)
        {
            if (createIfNotExists) return this.OpenFile(file).AsReadOnly();
            return (this as IReadOnlyDirectory).OpenFile(file);
        }

        IEnumerable<IReadOnlyDirectory> IReadOnlyDirectory.EnumerateDirectories()
        {
            return this.EnumerateDirectories().Select(d => d.AsReadOnly());
        }

        IEnumerable<IReadOnlyFile> IReadOnlyDirectory.EnumerateFiles()
        {
            return this.EnumerateFiles().Select(f => f.AsReadOnly());
        }

        IEnumerable<IReadOnlyFile> IReadOnlyDirectory.EnumerateFilesRecursive()
        {
            return this.EnumerateFilesRecursive().Select(f => f.AsReadOnly());
        }

        public IReadOnlyDirectory AsReadOnly() => this;
    }
}

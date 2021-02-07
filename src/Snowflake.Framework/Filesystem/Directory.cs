using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using Zio;
using System.Threading.Tasks;
using System.Threading;
using Snowflake.Persistence;
using Dapper;
using System.Text;

namespace Snowflake.Filesystem
{
    internal sealed class Directory : IDeletableDirectory, 
            IReadOnlyDirectory, IDirectory, IMoveFromableDirectory, IDeletableMoveFromableDirectory
    {
        internal static ConcurrentDictionary<string, object> DatabaseLocks = new ConcurrentDictionary<string, object>();

        private class ManifestRecord
        {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
            public string uuid { get; set; }
            public bool is_link { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
#pragma warning restore IDE1006 // Naming Styles
        }

        private SqliteDatabase? Manifest { get; }
        private object? DatabaseLock { get; }
        private bool IsDeleted { get; set; } = false;
        private bool UseManifest { get; set; } = true;

        internal Directory(string name, IFileSystem rootFs, DirectoryEntry parentDirectory, bool useManifest = true)
        {
            this.RootFileSystem = rootFs;
            this.ThisDirectory = parentDirectory.CreateSubdirectory(name);
            this.Name = name;
            this.UseManifest = useManifest;

            if (this.UseManifest)
            {
                var manifestPath = this.RootFileSystem.ConvertPathToInternal(this.ThisDirectory.Path / ".manifest");
                this.DatabaseLock = Directory.DatabaseLocks.GetOrAdd(manifestPath, new object());
                this.Manifest = new SqliteDatabase(manifestPath);
                lock (this.DatabaseLock)
                {
                    this.Manifest.CreateTable("directory_manifest", "uuid TEXT", "is_link BOOLEAN", "filename TEXT PRIMARY KEY");
                }
            }
        }

        internal Directory(IFileSystem rootFs)
        {
            this.RootFileSystem = rootFs;
            this.ThisDirectory = rootFs.GetDirectoryEntry("/");
            this.Name = "#FSROOT";
            this.UseManifest = true;
            var manifestPath = this.RootFileSystem.ConvertPathToInternal(this.ThisDirectory.Path / ".manifest");
            this.DatabaseLock = Directory.DatabaseLocks.GetOrAdd(manifestPath, new object());
            this.Manifest = new SqliteDatabase(manifestPath);
            lock (this.DatabaseLock)
            {
                this.Manifest.CreateTable("directory_manifest", "uuid TEXT", "is_link BOOLEAN", "filename TEXT PRIMARY KEY");
            }
        }

        private void CheckDeleted()
        {
            if (!this.ContainsFile(".manifest"))
            {
                this.IsDeleted = true;
            }

            if (this.IsDeleted)
            {
                throw new InvalidOperationException("Directory is already deleted.");
            }
        }

        internal void AddManifestRecord(string file, Guid guid, bool isLink)
        {
            this.CheckDeleted();
            if (!this.UseManifest) return;
            lock (this.DatabaseLock!)
            {
                this.Manifest?.Execute(connection =>
                {
                    connection.Execute(
                        @"INSERT OR REPLACE INTO directory_manifest (uuid, is_link, filename) VALUES (@guid, @isLink, @file)",
                        new { guid, isLink, file });
                });
            }
        }

        internal void RemoveManifestRecord(string file)
        {
            this.CheckDeleted();
            if (!this.UseManifest) return;
            lock (this.DatabaseLock!)
            {
                this.Manifest?.Execute(connection =>
                {
                    connection.Execute(@"DELETE FROM directory_manifest WHERE filename = @file", new { file });
                });
            }
        }

        internal (Guid guid, bool isLink) RetrieveManifestRecord(UPath file, bool updateLink)
        {
            this.CheckDeleted();
            var fileName = file.GetName();

            if (!this.UseManifest)
            {
                // Links are not supported in non-manifest directories.
                // i.e. Disposable or Projecting
                return (Guid.Empty, false);
            }

            lock (this.DatabaseLock!) 
            { 
                return this.Manifest!.Query<(Guid, bool)>(conn =>
                {
                    var record = conn.Query<ManifestRecord>(@"SELECT uuid, is_link FROM directory_manifest WHERE filename = @fileName",
                        new { fileName });

                    if (!record.Any() || updateLink)
                    {
                        // New file encountered.
                        bool isLink = this.CheckIsLink(file);
                        Guid guid = record.Any() ? new Guid(record.First().uuid) : Guid.NewGuid();
                        conn.Execute(@"INSERT OR REPLACE INTO directory_manifest (uuid, is_link, filename) VALUES (@guid, @isLink, @fileName)",
                            new { guid, fileName, isLink });

                        record = conn.Query<ManifestRecord>(@"SELECT uuid, is_link FROM directory_manifest WHERE filename = @fileName",
                            new { fileName });
                    }

                    var _record = record.First();
                    return (new Guid(_record.uuid), _record.is_link);
                });
            }
        }

        internal void UpdateLinkCache(UPath filePath, bool isLink)
        {
            this.CheckDeleted();

            lock (this.DatabaseLock!)
            {
                this.Manifest?.Execute(connection =>
                {
                    connection.Execute(
                        @"UPDATE directory_manifest SET is_link = @isLink WHERE filename = @file",
                        new { isLink, file = filePath.GetName() });
                });
            }
        }

        internal bool CheckIsLink(UPath file)
        {
            var entry = new FileEntry(this.RootFileSystem, file);

            // Uncreated files can not be links (but can link to nonexisting files).
            if (!entry.Exists) return false;

            try
            {
                using var stream = entry.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                Span<byte> buf = stackalloc byte[Link.LinkHeader.Length];
                stream.Read(buf);
                return Encoding.UTF8.GetString(buf) == Link.LinkHeader;
            } 
            catch
            {
                return false;
            }
        }

        internal IFileSystem RootFileSystem { get; }

        internal DirectoryEntry ThisDirectory { get; }
        //internal IFileSystem FileSystem { get; }

        public string Name { get; }

        public string RootedPath => this.ThisDirectory.Path.FullName!;

        public bool ContainsDirectory(string directory)
        {
            return this.RootFileSystem.DirectoryExists(this.ThisDirectory.Path / ((UPath)directory).ToRelative());
        }

        public bool ContainsFile(string file)
        {
            UPath filePath = ((UPath)file).ToRelative();
            var fullPath = this.ThisDirectory.Path / filePath;
            return this.RootFileSystem.FileExists(fullPath) 
                || this.RootFileSystem.DirectoryExists(fullPath);
        }

        public IDeletableDirectory OpenDirectory(string name)
        {
            this.CheckDeleted();
            var directoryStrings = ((UPath)name).Split();

            Directory currentDirectory = this;
            foreach (string directoryString in directoryStrings)
            {
                currentDirectory = new Directory(directoryString, this.RootFileSystem, currentDirectory.ThisDirectory, this.UseManifest);
            }
            return currentDirectory;
        }

        public IEnumerable<IDeletableDirectory> EnumerateDirectories()
        {
            this.CheckDeleted();
            return this.ThisDirectory.EnumerateDirectories()
                .Select(d => this.OpenDirectory(d.Name));
        }

        public IEnumerable<IFile> EnumerateFiles()
        {
            this.CheckDeleted();
            return this.ThisDirectory.EnumerateFiles()
                .Where(f => f.Name != ".manifest")
                .Select(f => this.OpenFile(f.Name, false));
        }

        private IFile OpenFile(UPath file, bool updateLink)
        {
            this.CheckDeleted();
            if (file.GetName() == ".manifest") throw new UnauthorizedAccessException("Unable to open manifest file.");

            (Guid guid, bool isLink) = this.RetrieveManifestRecord(file, updateLink);

            return isLink switch
            {
                false => new File(this, new FileEntry(this.RootFileSystem, file), guid),
                true => new Link(this, new FileEntry(this.RootFileSystem, file), guid)
            };
        }

        public DirectoryInfo UnsafeGetPath()
        {
            return new DirectoryInfo(this.RootFileSystem.ConvertPathToInternal(this.ThisDirectory.Path));
        }

        private IFile OpenFile(string file, bool updateLink)
        {
            return this.OpenFile(this.ThisDirectory.Path / Path.GetFileName(file), updateLink);
        }

        public IFile OpenFile(string file)
        {
            return this.OpenFile(this.ThisDirectory.Path / Path.GetFileName(file), false);
        }

        public IFile CopyFrom(FileInfo source) => this.CopyFrom(source, false);

        public IFile CopyFrom(FileInfo source, bool overwrite)
        {
            this.CheckDeleted();
            if (!source.Exists) throw new FileNotFoundException($"{source.FullName} could not be found.");
            string? fileName = Path.GetFileName(source.Name);

            if (fileName == null) throw new ArgumentException($"Could not get file name for path {source.Name}.");

            source.CopyTo(this.RootFileSystem.ConvertPathToInternal(this.ThisDirectory.Path / fileName), overwrite);
            return this.OpenFile(fileName, true);
        }

        public Task<IFile> CopyFromAsync(FileInfo source, CancellationToken cancellation = default) 
            => this.CopyFromAsync(source, false, cancellation);

        public async Task<IFile> CopyFromAsync(FileInfo source, bool overwrite, CancellationToken cancellation = default)
        {
            this.CheckDeleted();
            if (!source.Exists) throw new FileNotFoundException($"{source.FullName} could not be found.");
            string? fileName = Path.GetFileName(source.Name);
            if (fileName == null) throw new ArgumentException($"Cannot get file name for path {source.Name}");
            var file = this.OpenFile(fileName, false);
            if (file.Created && !overwrite) throw new IOException($"{source.Name} already exists in the target directory.");
            if (file is Link link)
            {
                file = link.UnsafeOpenAsFile();
            }
            using (var newStream = file.OpenStream(FileMode.Create, FileAccess.ReadWrite))
            using (var sourceStream = source.OpenRead())
            {
                await sourceStream.CopyToAsync(newStream, cancellation);
            }

            return this.OpenFile(fileName, true);
        }

        public IFile CopyFrom(IReadOnlyFile source, bool overwrite)
        {
            if (this.ContainsFile(source.Name) && !overwrite) throw new IOException($"{source.Name} already exists in the target directory.");
            this.AddManifestRecord(source.Name, source.FileGuid, source.IsLink);

            return this.CopyFrom(source.UnsafeGetFilePointerPath(), overwrite);
        }

        public Task<IFile> CopyFromAsync(IReadOnlyFile source, CancellationToken cancellation = default) 
            => this.CopyFromAsync(source, false, cancellation);

        public async Task<IFile> CopyFromAsync(IReadOnlyFile source, bool overwrite, CancellationToken cancellation = default)
        {

            if (this.ContainsFile(source.Name) && !overwrite) throw new IOException($"{source.Name} already exists in the target directory");
            this.AddManifestRecord(source.Name, source.FileGuid, source.IsLink);
            return await this.CopyFromAsync(source.UnsafeGetFilePointerPath(), overwrite, cancellation);
        }

        public IFile CopyFrom(IReadOnlyFile source) => this.CopyFrom(source, false);

        public IFile MoveFrom(IFile source) => this.MoveFrom(source, false);

        public IFile MoveFrom(IFile source, bool overwrite)
        {
            this.CheckDeleted();

            if (!source.Created) throw new FileNotFoundException($"{source.UnsafeGetFilePointerPath().FullName} could not be found.");
            if (this.ContainsFile(source.Name) && !overwrite) throw new IOException($"{source.Name} already exists in the target directory");
            this.AddManifestRecord(source.Name, source.FileGuid, source.IsLink);
            var file = this.OpenFile(source.Name, false);
            // unsafe usage here as optimization.
            source.UnsafeGetFilePointerPath().MoveTo(file.UnsafeGetFilePointerPath().ToString(), overwrite);
            source.Delete();
            return this.OpenFile(file.Name, true);
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

            Queue<IDeletableDirectory> dirsToProcess = new Queue<IDeletableDirectory>(queuedDirs);

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

        IReadOnlyFile IReadOnlyDirectory.OpenFile(string file)
        {
            if (this.ContainsFile(Path.GetFileName(file))) return this.OpenFile(file, false).AsReadOnly();
            throw new FileNotFoundException($"File {file} does not exist within the directory {this.Name}.");
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

        public void Delete()
        {
            this.CheckDeleted();
            this.IsDeleted = true;
            if (this.UseManifest)
            {
                lock (this.DatabaseLock!)
                {
                    this.ThisDirectory.Delete(true);
                }
            }
            else
            {
                this.ThisDirectory.Delete(true);
            }
        }

        public IFile LinkFrom(FileInfo source) => this.LinkFrom(source, false);

        public IFile LinkFrom(FileInfo source, bool overwrite)
        {

            if (!source.Exists) throw new FileNotFoundException($"{source.FullName} could not be found.");
            string? fileName = Path.GetFileName(source.Name);

            if (fileName == null) throw new ArgumentException($"Could not get file name for path {source.Name}.");

            if (this.ContainsFile(fileName) && !overwrite) 
                throw new IOException($"{source.Name} already exists in the target directory.");

            UPath destination = (this.ThisDirectory.Path / Path.GetFileName(fileName));

            // Ensure link cache is cleared
            this.UpdateLinkCache(destination, true);

            // Create an orphaned file
            var file = new File(this, new FileEntry(this.RootFileSystem, destination), Guid.Empty);

            using (var newStream = file.OpenStream(FileMode.Create, FileAccess.ReadWrite))
            {
                newStream.Write(Encoding.UTF8.GetBytes($"{Link.LinkHeader}{Path.GetFullPath(new Uri(source.FullName).LocalPath)}"));
            }

            // De-orphan the file (as a link)
            return this.OpenFile(fileName, true);
        }

        IReadOnlyDirectory IMutableDirectoryBase<IDeletableDirectory, IReadOnlyDirectory>.ReopenAs()
            => this;

        IMoveFromableDirectory IMutableDirectoryBase<IDeletableDirectory, IMoveFromableDirectory>.ReopenAs()
            => this;

        IDirectory IMutableDirectoryBase<IDeletableDirectory, IDirectory>.ReopenAs()
            => this;

        IDeletableMoveFromableDirectory IMutableDirectoryBase<IDeletableDirectory, IDeletableMoveFromableDirectory>.ReopenAs()
            => this;

        IDisposableDirectory IMutableDirectoryBase<IDeletableDirectory, IDisposableDirectory>.ReopenAs()
        {
            if (this.EnumerateDirectories().Any() || this.EnumerateFiles().Any())
            {
                throw new IOException("The directory is not empty. Disposable directories must not contain files prior to opening.");
            }

            if (this.UseManifest)
            {
                lock (this.DatabaseLock!)
                {
                    // Recreate the directory without manifest
                    this.ThisDirectory.Delete();
                    this.ThisDirectory.Create();
                }
            }
            this.UseManifest = false;
            return new DisposableDirectory(this);
        }

        IReadOnlyFile IReadOnlyDirectory.OpenFile(string file, bool openIfNotExists)
        {
            if (openIfNotExists) return this.OpenFile(file, false).AsReadOnly();
            return (this as IReadOnlyDirectory).OpenFile(file);
        }
    }
}


﻿using System;
using System.Collections.Generic;
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
    internal sealed class Directory : IDirectory, IReadOnlyDirectory
    {
        private class ManifestRecord
        {
            public string uuid { get; set; }
            public bool is_link { get; set; }
        }

        private SqliteDatabase Manifest { get; }

        private bool IsDeleted { get; set; } = false;

        internal Directory(string name, IFileSystem rootFs, DirectoryEntry parentDirectory)
        {
            this.RootFileSystem = rootFs;
            this.ThisDirectory = parentDirectory.CreateSubdirectory(name);
            this.Name = name;

            this.Manifest =
                new SqliteDatabase(this.RootFileSystem.ConvertPathToInternal(this.ThisDirectory.Path / ".manifest"));
            this.Manifest.CreateTable("directory_manifest", "uuid TEXT", "is_link BOOLEAN", "filename TEXT PRIMARY KEY");
        }

        internal Directory(IFileSystem rootFs)
        {
            this.RootFileSystem = rootFs;
            this.ThisDirectory = rootFs.GetDirectoryEntry("/");
            this.Name = "#FSROOT";

            this.Manifest =
                new SqliteDatabase(this.RootFileSystem.ConvertPathToInternal(this.ThisDirectory.Path / ".manifest"));
            this.Manifest.CreateTable("directory_manifest", "uuid UUID", "is_link BOOLEAN", "filename TEXT PRIMARY KEY");
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
            this.Manifest.Execute(connection =>
            {
                connection.Execute(
                    @"INSERT OR REPLACE INTO directory_manifest (uuid, is_link, filename) VALUES (@guid, @isLink, @file)",
                    new {guid, isLink, file});
            });
        }

        internal void RemoveManifestRecord(string file)
        {
            this.CheckDeleted();
            this.Manifest.Execute(connection =>
            {
                connection.Execute(@"DELETE FROM directory_manifest WHERE filename = @file", new {file});
            });
        }

        internal (Guid guid, bool isLink) RetrieveManifestRecord(UPath file)
        {
            this.CheckDeleted();
            var fileName = file.GetName();
            return this.Manifest.Query<(Guid, bool)>(conn =>
            {
                var record = conn.Query<ManifestRecord>(@"SELECT uuid, is_link FROM directory_manifest WHERE filename = @fileName",
                    new {fileName});
             
                if (record.Count() == 0)
                {
                    // New file encountered.

                    bool isLink = this.CheckIsLink(file);
                    conn.Execute(@"INSERT OR REPLACE INTO directory_manifest (uuid, is_link, filename) VALUES (@guid, @isLink, @fileName)",
                        new {guid = Guid.NewGuid(), fileName, isLink});

                    record = conn.Query<ManifestRecord>(@"SELECT uuid FROM directory_manifest WHERE filename = @fileName",
                        new {fileName});
                }

                var _record = record.First();
                return (new Guid(_record.uuid), _record.is_link);
            });
        }

        internal bool CheckIsLink(UPath file)
        {
            var entry = new FileEntry(this.RootFileSystem, file);

            // Uncreated files can not be links (but can link to nonexisting files).
            if (!entry.Exists) return false;

            try
            {
                using var stream = entry.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                Span<byte> buf = stackalloc byte[5];
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
            this.CheckDeleted();
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
            this.CheckDeleted();
            return this.ThisDirectory.EnumerateDirectories()
                .Select(d => this.OpenDirectory(d.Name));
        }

        public IEnumerable<IFile> EnumerateFiles()
        {
            this.CheckDeleted();
            return this.ThisDirectory.EnumerateFiles()
                .Where(f => f.Name != ".manifest")
                .Select(f => this.OpenFile(f.Name));
        }

        private IFile OpenFile(UPath file)
        {
            this.CheckDeleted();
            if (file.GetName() == ".manifest") throw new UnauthorizedAccessException("Unable to open manifest file.");

            (Guid guid, bool isLink) = this.RetrieveManifestRecord(file);

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

        public IFile OpenFile(string file)
        {
            return this.OpenFile(this.ThisDirectory.Path / Path.GetFileName(file));
        }

        public IFile CopyFrom(FileInfo source) => this.CopyFrom(source, false);

        public IFile CopyFrom(FileInfo source, bool overwrite)
        {
            this.CheckDeleted();
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
            this.CheckDeleted();
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
            this.AddManifestRecord(source.Name, source.FileGuid, false);

            return this.CopyFrom(source.UnsafeGetFilePointerPath(), overwrite);
        }

        public Task<IFile> CopyFromAsync(IReadOnlyFile source, CancellationToken cancellation = default) 
            => this.CopyFromAsync(source, false, cancellation);

        public async Task<IFile> CopyFromAsync(IReadOnlyFile source, bool overwrite, CancellationToken cancellation = default)
        {

            if (this.ContainsFile(source.Name) && !overwrite) throw new IOException($"{source.Name} already exists in the target directory");
            this.AddManifestRecord(source.Name, source.FileGuid, false);
            return await this.CopyFromAsync(source.UnsafeGetFilePointerPath(), overwrite, cancellation);
        }

        public IFile CopyFrom(IReadOnlyFile source) => this.CopyFrom(source, false);

        public IFile MoveFrom(IFile source) => this.MoveFrom(source, false);

        public IFile MoveFrom(IFile source, bool overwrite)
        {
            this.CheckDeleted();

            if (!source.Created) throw new FileNotFoundException($"{source.UnsafeGetFilePointerPath().FullName} could not be found.");
            if (this.ContainsFile(source.Name) && !overwrite) throw new IOException($"{source.Name} already exists in the target directory");
            this.AddManifestRecord(source.Name, source.FileGuid, false);
            var file = this.OpenFile(source.Name);
            // unsafe usage here as optimization.
            source.UnsafeGetFilePointerPath().MoveTo(file.UnsafeGetFilePointerPath().ToString(), overwrite);
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

        public void Delete()
        {
            this.CheckDeleted();
            this.IsDeleted = true;
            this.ThisDirectory.Delete(true);
        }
    }
}

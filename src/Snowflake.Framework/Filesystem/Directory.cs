using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Zio;
using Tsuku.Extensions;

namespace Snowflake.Filesystem
{
    internal sealed partial class Directory : IDeletableDirectory, 
            IReadOnlyDirectory, IDirectory, IMoveFromableDirectory, IDeletableMoveFromableDirectory
    {
        internal bool IsDeleted { get; set; } = false;
       

        internal Directory(string name, IFileSystem rootFs, DirectoryEntry parentDirectory, bool useManifest = true)
        {
            this.RootFileSystem = rootFs;
            this.ThisDirectory = parentDirectory.CreateSubdirectory(name);
            this.Name = name;
        }

        internal Directory(IFileSystem rootFs)
        {
            this.RootFileSystem = rootFs;
            this.ThisDirectory = rootFs.GetDirectoryEntry("/");
            this.Name = "#FSROOT";
        }

        internal IFileSystem RootFileSystem { get; }

        internal DirectoryEntry ThisDirectory { get; }
        //internal IFileSystem FileSystem { get; }

        public string Name { get; }

        public string RootedPath => this.ThisDirectory.Path.FullName!;

        private void CheckDeleted()
        {
            // normal doesn't check symlinks
            if (!new DirectoryInfo(this.RootFileSystem.ConvertPathToInternal(this.ThisDirectory.Path)).Exists())
            {
                this.IsDeleted = true;
            }

            if (this.IsDeleted)
            {
                throw new InvalidOperationException("Directory is already deleted.");
            }
        }

        public bool ContainsDirectory(string directory)
        {
            UPath fullPath = this.ThisDirectory.Path / ((UPath)directory).ToRelative();
            string realPath = this.RootFileSystem.ConvertPathToInternal(fullPath);
            return new DirectoryInfo(realPath).Exists();
        }

        public bool ContainsFile(string file)
        {
            UPath filePath = ((UPath)file).ToRelative();
            var fullPath = this.ThisDirectory.Path / filePath;
            string realPath = this.RootFileSystem.ConvertPathToInternal(fullPath);
            return
                new DirectoryInfo(realPath).Exists()
                || new FileInfo(realPath).Exists();
        }

        internal Directory OpenDirectory(string name)
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

        public IFile OpenFile(string file) => this.OpenFile(file, Guid.NewGuid());

        private IFile OpenFile(string file, Guid inheritGuid)
        {
            return this.OpenFile(this.ThisDirectory.Path / Path.GetFileName(file), inheritGuid);
        }

        private IFile OpenFile(UPath file, Guid inheritGuid)
        {
            this.CheckDeleted();
            if (this.ContainsDirectory(file.GetName())) throw new IOException("Tried to open a directory as a file.");
            var fileEntry = new FileEntry(this.RootFileSystem, file);
            var rawInfo = new FileInfo(this.RootFileSystem.ConvertPathToInternal(fileEntry.Path));
            if (rawInfo.TryGetGuidAttribute(File.SnowflakeFile, out Guid guid))
            {
                return new File(this, fileEntry, guid);
            }
            guid = inheritGuid;
            if (rawInfo.Exists)
            {
                rawInfo.SetAttribute(File.SnowflakeFile, guid);
            }
            return new File(this, fileEntry, guid);
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
                .Select(f => this.OpenFile(f.Name));
        }

        public DirectoryInfo UnsafeGetPath()
        {
            return new DirectoryInfo(this.RootFileSystem.ConvertPathToInternal(this.ThisDirectory.Path));
        }

        public void Delete()
        {
            this.CheckDeleted();
            this.IsDeleted = true;
            this.ThisDirectory.Delete(true);
        }

        IReadOnlyDirectory IDirectoryOpeningDirectoryBase<IReadOnlyDirectory>.OpenDirectory(string name)
        {
            if (this.ContainsDirectory(name)) return (Directory)this.OpenDirectory(name);
            throw new DirectoryNotFoundException($"Directory {name} does not exist within {this.Name}.");
        }

        IReadOnlyFile IFileOpeningDirectoryBase<IReadOnlyFile>.OpenFile(string file)
        {
            if (this.ContainsFile(Path.GetFileName(file))) return this.OpenFile(file).AsReadOnly();
            throw new FileNotFoundException($"File {file} does not exist within the directory {this.Name}.");
        }

        IEnumerable<IReadOnlyDirectory> IEnumerableDirectoryBase<IReadOnlyDirectory, IReadOnlyFile>.EnumerateDirectories()
        {
            return this.EnumerateDirectories().Select(d => d.AsReadOnly());
        }

        IEnumerable<IReadOnlyFile> IEnumerableDirectoryBase<IReadOnlyDirectory, IReadOnlyFile>.EnumerateFiles()
        {
            return this.EnumerateFiles().Select(f => f.AsReadOnly());
        }

        IEnumerable<IReadOnlyFile> IEnumerableDirectoryBase<IReadOnlyDirectory, IReadOnlyFile>.EnumerateFilesRecursive()
        {
            return this.EnumerateFilesRecursive().Select(f => f.AsReadOnly());
        }

        IReadOnlyDirectory IReopenableDirectoryBase<IReadOnlyDirectory>.ReopenAs()
            => this;

        IMoveFromableDirectory IReopenableDirectoryBase<IMoveFromableDirectory>.ReopenAs()
            => this;

        IDirectory IReopenableDirectoryBase<IDirectory>.ReopenAs()
            => this;

        IDeletableMoveFromableDirectory IReopenableDirectoryBase<IDeletableMoveFromableDirectory>.ReopenAs()
            => this;

        IDisposableDirectory IReopenableDirectoryBase<IDisposableDirectory>.ReopenAs()
        {
            if (this.EnumerateDirectories().Any() || this.EnumerateFiles().Any())
            {
                throw new IOException("The directory is not empty. Disposable directories must not contain files prior to opening.");
            }

            return new DisposableDirectory(this);
        }

        IReadOnlyFile IReadOnlyDirectory.OpenFile(string file, bool openIfNotExists)
        {
            if (openIfNotExists) return this.OpenFile(file).AsReadOnly();
            return (this as IReadOnlyDirectory).OpenFile(file);
        }

        IDeletableDirectory IDirectoryOpeningDirectoryBase<IDeletableDirectory>.OpenDirectory(string name)
            => this.OpenDirectory(name);

        public static bool IsValidFileName(string name)
        {
            return !String.IsNullOrEmpty(name) && name.IndexOfAny(Path.GetInvalidFileNameChars()) == -1;
        }
    }
}


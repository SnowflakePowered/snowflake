using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Zio;
using Emet.FileSystems;

namespace Snowflake.Filesystem
{
    internal sealed class File : IFile
    {
        internal File(Directory parentDirectory, FileEntry file, Guid guid)
        {
            this.RawInfo = file;
            this.ParentDirectory = parentDirectory;
            this.FileGuid = guid;
        }

        public string Name => this.RawInfo.Name;

        internal FileEntry RawInfo { get; private set; }

        public long Length 
        { 
            get 
            {
                var fileInfo = this.UnsafeGetFilePath();
                var dirEntry = new Emet.FileSystems.DirectoryEntry(fileInfo.FullName, FileSystem.FollowSymbolicLinks.Always);
                return dirEntry.FileType == FileType.File ? dirEntry.FileSize : -1; 
            } 
        }

        internal Directory ParentDirectory { get; }

        public bool Created 
        {
            get
            {
                var fileInfo = this.UnsafeGetFilePath();
                var dirEntry = new Emet.FileSystems.DirectoryEntry(fileInfo.FullName, FileSystem.FollowSymbolicLinks.Always);
                return dirEntry.FileType == FileType.File;
            } 
        }

        public Guid FileGuid { get; }

        public Stream OpenStream()
        {
            return this.OpenStream(FileAccess.ReadWrite);
        }

        public Stream OpenStream(FileAccess rw)
        {
            return this.OpenStream(FileMode.OpenOrCreate, rw);
        }

        public Stream OpenStream(FileMode mode, FileAccess rw, FileShare share = FileShare.None)
        {
            return this.RawInfo.Open(mode, rw, share);
        }

        public void Rename(string newName)
        {
            this.RawInfo.MoveTo(this.RawInfo.Parent!.Path / Path.GetFileName(newName));
            this.ParentDirectory.RemoveManifestRecord(this.Name);
            this.RawInfo = new FileEntry(this.RawInfo.FileSystem, this.RawInfo.Parent.Path / Path.GetFileName(newName));
            this.ParentDirectory.AddManifestRecord(this.Name, this.FileGuid, false);
        }

        public void Delete()
        {
            if (this.RawInfo.Exists)
            {
                this.RawInfo.Delete();
            }

            this.ParentDirectory.RemoveManifestRecord(this.Name);
        }

        public FileInfo UnsafeGetFilePath()
        {
            return new FileInfo(this.RawInfo.FileSystem.ConvertPathToInternal(this.RawInfo.Path));
        }

        FileInfo IReadOnlyFile.UnsafeGetFilePointerPath() => this.UnsafeGetFilePath();

        public Stream OpenReadStream()
        {
            if (!this.Created) throw new FileNotFoundException($"The file {this.Name} was not found");
            return this.OpenStream(FileAccess.Read);
        }

        public IReadOnlyFile AsReadOnly() => this;

        public string RootedPath => this.RawInfo.Path.ToString();

        IReadOnlyDirectory IReadOnlyFile.ParentDirectory => this.ParentDirectory;

        public bool IsLink => false;
    }
}

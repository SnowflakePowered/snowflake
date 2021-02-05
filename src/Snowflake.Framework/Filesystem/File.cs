using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Zio;

namespace Snowflake.Filesystem
{
    internal sealed class File : IFile
    {
        internal File(Directory parentDirectory, FileEntry file, Guid guid)
        {
            this.RawInfo = file;
            this._ParentDirectory = parentDirectory;
            this.FileGuid = guid;
        }

        public string Name => this.RawInfo.Name;

        internal FileEntry RawInfo { get; private set; }

        public long Length => this.RawInfo.Exists ? this.RawInfo.Length : -1;

        internal Directory _ParentDirectory { get; }
        public IDirectory ParentDirectory => _ParentDirectory;

        public bool Created => this.RawInfo.Exists;

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
            this._ParentDirectory.RemoveManifestRecord(this.Name);
            this.RawInfo = new FileEntry(this.RawInfo.FileSystem, this.RawInfo.Parent.Path / Path.GetFileName(newName));
            this._ParentDirectory.AddManifestRecord(this.Name, this.FileGuid, false);
        }

        public void Delete()
        {
            if (this.RawInfo.Exists)
            {
                this.RawInfo.Delete();
            }

            this._ParentDirectory.RemoveManifestRecord(this.Name);
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

        IReadOnlyDirectory IReadOnlyFile.ParentDirectory => this._ParentDirectory;

        public bool IsLink => false;
    }
}

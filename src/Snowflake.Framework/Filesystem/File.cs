using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Zio;

namespace Snowflake.Filesystem
{
    internal sealed class File : IFile, IReadOnlyFile
    {
        internal File(Directory parentDirectory, FileEntry file, Guid guid)
        {
            this.RawInfo = file;
            this._ParentDirectory = parentDirectory;
            this.FileGuid = guid;
        }

        public string Name => this.RawInfo.Name;

        internal FileEntry RawInfo { get; private set; }

        public long Length => this.RawInfo.Length;

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
            return this.RawInfo.Open(FileMode.OpenOrCreate, rw);
        }

        public void Rename(string newName)
        {
            this.RawInfo.MoveTo(this.RawInfo.Parent.Path / Path.GetFileName(newName));
            this._ParentDirectory.RemoveGuid(this.Name);
            this.RawInfo = new FileEntry(this.RawInfo.FileSystem, this.RawInfo.Parent.Path / Path.GetFileName(newName));
            this._ParentDirectory.AddGuid(this.Name, this.FileGuid);
        }

        public void Delete()
        {
            if (this.RawInfo.Exists)
            {
                this.RawInfo.Delete();
            }

            this._ParentDirectory.RemoveGuid(this.Name);
        }

        public FileInfo UnsafeGetFilePath()
        {
            return new FileInfo(this.RawInfo.FileSystem.ConvertPathToInternal(this.RawInfo.Path));
        }

        public Stream OpenReadStream()
        {
            if (!this.Created) throw new FileNotFoundException($"The file {this.Name} was not found");
            return this.OpenStream(FileAccess.Read);
        }

        public IReadOnlyFile AsReadOnly() => this;

        public string RootedPath => this.RawInfo.Path.ToString();

        IReadOnlyDirectory IReadOnlyFile.ParentDirectory => this._ParentDirectory;
    }
}

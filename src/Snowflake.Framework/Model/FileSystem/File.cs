using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Zio;

namespace Snowflake.Model.FileSystem
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

        internal FileEntry RawInfo { get; }

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
        }

        public void Delete()
        {
            if (this.RawInfo.Exists)
            {
                this.RawInfo.Delete();
            }
            this._ParentDirectory.RemoveGuid(this.Name);
        }

        public FileInfo GetFilePath()
        {
            return new FileInfo(this.RawInfo.FileSystem.
                ConvertPathToInternal(this.RawInfo.Path));
        }

        public string RootedPath => this.RawInfo.Path.ToString();
    }
}

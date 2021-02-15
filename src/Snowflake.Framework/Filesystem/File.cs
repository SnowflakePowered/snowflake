using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Zio;
using Emet.FileSystems;
using Tsuku.Extensions;

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

        public long Length => this.UnsafeGetPath().Length();

        internal Directory ParentDirectory { get; }

        public bool Created => this.UnsafeGetPath().ContentExists();

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
            var stream = this.RawInfo.Open(mode, rw, share);
            this.ParentDirectory.FileGuidProvider.SetGuid(this.UnsafeGetPath(), this.FileGuid);
            return stream;
        }

        public void Rename(string newName)
        {
            this.RawInfo.MoveTo(this.RawInfo.Parent!.Path / Path.GetFileName(newName));
            this.RawInfo = new FileEntry(this.RawInfo.FileSystem, this.RawInfo.Parent.Path / Path.GetFileName(newName));
        }

        public void Delete()
        {
            if (this.RawInfo.Exists)
            {
                this.RawInfo.Delete();
            }
        }

        public FileInfo UnsafeGetPath()
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

        IReadOnlyDirectory IReadOnlyFile.ParentDirectory => this.ParentDirectory;

        public bool IsLink => false;
    }
}

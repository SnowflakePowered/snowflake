using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Zio;

namespace Snowflake.Filesystem
{
    internal sealed class Link : IFile
    {
        internal static readonly string LinkHeader = "@@SF__LINK\n";
        internal Link(Directory parentDirectory, FileEntry file, Guid guid)
        {
            this.RawInfo = file;
            this.FileSystemPath = new Lazy<FileInfo>(this.GetFileInfo);
            this._ParentDirectory = parentDirectory;
            this.FileGuid = guid;
        }

        private FileInfo GetFileInfo() => new FileInfo(this.RawInfo.ReadAllText(Encoding.UTF8)[LinkHeader.Length..]);

        public string Name => this.RawInfo.Name;

        internal FileEntry RawInfo { get; private set; }
        private Lazy<FileInfo> FileSystemPath { get; }

        public long Length => this.Created ? this.FileSystemPath.Value.Length : -1;

        internal Directory _ParentDirectory { get; }
        public IDirectory ParentDirectory => _ParentDirectory;

        public bool Created => this.FileSystemPath.Value.Exists;

        public Guid FileGuid { get; }

        public Stream OpenStream()
        {
            return this.OpenStream(FileAccess.ReadWrite);
        }

        public Stream OpenStream(FileAccess rw)
        {
            return this.OpenStream(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }

        public void Rename(string newName)
        {
            this.RawInfo.MoveTo(this.RawInfo.Parent!.Path / Path.GetFileName(newName));
            this._ParentDirectory.RemoveManifestRecord(this.Name);
            this.RawInfo = new FileEntry(this.RawInfo.FileSystem, this.RawInfo.Parent.Path / Path.GetFileName(newName));
            this._ParentDirectory.AddManifestRecord(this.Name, this.FileGuid, true);
        }

        public void Delete()
        {
            if (this.RawInfo.Exists)
            {
                this.RawInfo.Delete();
            }

            this._ParentDirectory.RemoveManifestRecord(this.Name);
        }

        public FileInfo UnsafeGetFilePath() => this.FileSystemPath.Value;

        FileInfo IReadOnlyFile.UnsafeGetFilePointerPath() => new FileInfo(this.RawInfo.FileSystem.ConvertPathToInternal(this.RawInfo.Path));

        public Stream OpenReadStream()
        {
            if (!this.Created) throw new FileNotFoundException($"The file {this.Name} was not found");
            return this.OpenStream(FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public IReadOnlyFile AsReadOnly() => this;

        public Stream OpenStream(FileMode mode, FileAccess rw, FileShare share)
        {
            return this.FileSystemPath.Value.Open(mode, rw, share);
        }

        public string RootedPath => this.RawInfo.Path.ToString();

        IReadOnlyDirectory IReadOnlyFile.ParentDirectory => this._ParentDirectory;

        /// <summary>
        /// Open the <see cref="Link"/> as a <see cref="File"/>, exposing
        /// the link source file.
        /// </summary>
        /// <returns>A <see cref="File"/> where the link file is readable.</returns>
        internal File UnsafeOpenAsFile() => new File(this._ParentDirectory, this.RawInfo, this.FileGuid);

        public bool IsLink => true;
    }
}
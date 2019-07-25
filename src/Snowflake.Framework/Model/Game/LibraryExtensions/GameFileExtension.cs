using System;
using System.Collections.Generic;
using Snowflake.Model.Database;
using Snowflake.Filesystem;
using Snowflake.Model.Records.File;
using Zio;

namespace Snowflake.Model.Game.LibraryExtensions
{
    internal class GameFileExtension : IGameFileExtension
    {
        public GameFileExtension(IFileSystem gameFsRoot, FileRecordLibrary files)
        {
            this.Root = new Directory(gameFsRoot);
            this.FileRecordLibrary = files;
            this.SavesRoot = this.Root.OpenDirectory("saves");
            this.ProgramRoot = this.Root.OpenDirectory("program");
            this.MediaRoot = this.Root.OpenDirectory("media");
            this.ResourceRoot = this.Root.OpenDirectory("resource");
            this.RuntimeRoot = this.Root.OpenDirectory("runtime");
            this.MiscRoot = this.Root.OpenDirectory("misc");
        }

        private IDirectory Root { get; }

        private FileRecordLibrary FileRecordLibrary { get; }

        public IDirectory SavesRoot { get; }

        public IDirectory ProgramRoot { get; }

        public IDirectory MediaRoot { get; }

        public IDirectory MiscRoot { get; }

        public IDirectory ResourceRoot { get; }

        public IDirectory RuntimeRoot { get; }

        public IEnumerable<IFileRecord> FileRecords => this.FileRecordLibrary.GetFileRecords(this.Root);

        public IDirectory GetRuntimeLocation()
        {
            return this.RuntimeRoot.OpenDirectory(Guid.NewGuid().ToString());
        }

        public IFileRecord? GetFileInfo(IFile file) => this.FileRecordLibrary.GetRecord(file);

        public IFileRecord RegisterFile(IFile file, string mimetype)
        {
            this.FileRecordLibrary.RegisterFile(file, mimetype);
            return this.GetFileInfo(file)!;
        }
    }
}

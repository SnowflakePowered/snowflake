using System;
using System.Collections.Generic;
using Snowflake.Model.Database;
using Snowflake.Filesystem;
using Snowflake.Model.Records.File;
using Zio;
using System.Threading.Tasks;

namespace Snowflake.Model.Game.LibraryExtensions
{
    internal class GameFileExtension : IGameFileExtension
    {
        public GameFileExtension(IDirectory gameFsRoot, FileRecordLibrary files)
        {
            this._Root = gameFsRoot;
            this.FileRecordLibrary = files;
            this.SavesRoot = this._Root.OpenDirectory("saves");
            this.ProgramRoot = this._Root.OpenDirectory("program");
            this.MediaRoot = this._Root.OpenDirectory("media");
            this.ResourceRoot = this._Root.OpenDirectory("resource");
            this.RuntimeRoot = this._Root.OpenDirectory("runtime");
            this.MiscRoot = this._Root.OpenDirectory("misc");
        }

        private IDirectory _Root { get; }

        public IReadOnlyDirectory Root => this._Root.AsReadOnly();
        private FileRecordLibrary FileRecordLibrary { get; }

        public IDirectory SavesRoot { get; }

        public IDirectory ProgramRoot { get; }

        public IDirectory MediaRoot { get; }

        public IDirectory MiscRoot { get; }

        public IDirectory ResourceRoot { get; }

        public IDirectory RuntimeRoot { get; }

        public IDeletableDirectory GetRuntimeLocation()
        {
            return this.RuntimeRoot.OpenDirectory(Guid.NewGuid().ToString());
        }

        public IFileRecord? GetFileInfo(IFile file) => this.FileRecordLibrary.GetRecord(file);

        public IFileRecord RegisterFile(IFile file, string mimetype)
        {
            this.FileRecordLibrary.RegisterFile(file, mimetype);
            return this.GetFileInfo(file)!;
        }

        public IEnumerable<IFileRecord> GetFileRecords() => this.FileRecordLibrary.GetFileRecords(this._Root);

        public IAsyncEnumerable<IFileRecord> GetFileRecordsAsync() => this.FileRecordLibrary.GetFileRecordsAsync(this._Root);

        public Task<IFileRecord?> GetFileInfoAsync(IFile file) => this.FileRecordLibrary.GetRecordAsync(file);

        public async Task<IFileRecord> RegisterFileAsync(IFile file, string mimetype)
        {
            await this.FileRecordLibrary.RegisterFileAsync(file, mimetype);
            return (await this.GetFileInfoAsync(file))!;
        }
    }
}

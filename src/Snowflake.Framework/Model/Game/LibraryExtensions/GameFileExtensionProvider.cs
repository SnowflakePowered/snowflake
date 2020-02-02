using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Model.Database;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Model.Records.File;
using Snowflake.Model.Records.Game;
using Zio;

namespace Snowflake.Model.Game.LibraryExtensions
{
    internal class GameFileExtensionProvider : IGameFileExtensionProvider
    {
        public GameFileExtensionProvider
        (FileRecordLibrary fileLibrary,
            IFileSystem gameFolderFs)
        {
            this.FileLibrary = fileLibrary;
            this.GameFolderRoot = gameFolderFs;
        }

        internal FileRecordLibrary FileLibrary { get; }

        internal IFileSystem GameFolderRoot { get; }

        public IGameFileExtension MakeExtension(IGameRecord record)
        {
            var gameFsRoot = this.GameFolderRoot
                .GetOrCreateSubFileSystem((UPath) "/" / record.RecordID.ToString());
            return new GameFileExtension(gameFsRoot, this.FileLibrary);
        }

        public void UpdateFile(IFileRecord file)
        {
            this.FileLibrary.UpdateRecord(file);
        }

        public Task UpdateFileAsync(IFileRecord file)
        {
            return this.FileLibrary.UpdateRecordAsync(file);
        }

        IGameExtension IGameExtensionProvider.MakeExtension(IGameRecord record)
        {
            return this.MakeExtension(record);
        }
    }
}

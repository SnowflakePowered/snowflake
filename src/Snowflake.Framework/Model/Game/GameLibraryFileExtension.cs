using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Model.Database;
using Snowflake.Model.Game.Extensions;
using Snowflake.Model.Records.File;
using Snowflake.Model.Records.Game;
using Zio;

namespace Snowflake.Model.Game
{
    internal class GameLibraryFileExtension : IGameLibraryExtension<IGameFileExtension>
    {
        public GameLibraryFileExtension
            (FileRecordLibrary fileLibrary,
            IFileSystem gameFolderFs)
        {
            this.FileLibrary = fileLibrary;
            this.GameFolderRoot = gameFolderFs;
        }

        internal FileRecordLibrary FileLibrary { get; }

        internal IFileSystem GameFolderRoot{ get; }

        public IGameFileExtension MakeExtension(IGameRecord record)
        {
            var gameFsRoot = this.GameFolderRoot
                .GetOrCreateSubFileSystem((UPath)"/" / record.RecordId.ToString());
            return new GameFileExtension(gameFsRoot, this.FileLibrary);
        }

        public void UpdateFile(IFileRecord file)
        {
            this.FileLibrary.UpdateRecord(file);
        }

        IGameExtension IGameLibraryExtension.MakeExtension(IGameRecord record)
        {
            return this.MakeExtension(record);
        }
    }
}

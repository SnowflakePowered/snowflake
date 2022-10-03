using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Filesystem.Library;
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
            ContentLibraryStore contentLibrary)
        {
            this.FileLibrary = fileLibrary;
            this.ContentLibrary = contentLibrary;
        }

        internal FileRecordLibrary FileLibrary { get; }

        internal ContentLibraryStore ContentLibrary { get; }
        public IGameFileExtension MakeExtension(IGameRecord record)
        {
            var libraryRoot = this.ContentLibrary.GetRecordLibrary(record);

            if (libraryRoot == null)
            {
                throw new FileNotFoundException("Game is missing associated content library.");
            }
            return new GameFileExtension(libraryRoot.OpenRecordLibrary(record.RecordID), this.FileLibrary);
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

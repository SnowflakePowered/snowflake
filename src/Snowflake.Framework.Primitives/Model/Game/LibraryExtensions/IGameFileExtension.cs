using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Filesystem;
using Snowflake.Model.Records.File;

namespace Snowflake.Model.Game.LibraryExtensions
{
    public interface IGameFileExtensionProvider : IGameExtensionProvider<IGameFileExtension>
    {
        void UpdateFile(IFileRecord file);
    }

    public interface IGameFileExtension : IGameExtension
    {
        IDirectory SavesRoot { get; }
        IDirectory ProgramRoot { get; }
        IDirectory MediaRoot { get; }
        IDirectory MiscRoot { get; }
        IDirectory ResourceRoot { get; }
        IDirectory RuntimeRoot { get; }

        IEnumerable<IFileRecord> Files { get; }

        IFileRecord? GetFileInfo(IFile file);

        IFileRecord RegisterFile(IFile file, string mimetype);

        IDirectory GetSavesLocation(string saveType);

        IDirectory GetRuntimeLocation();
    }

    public static class GameFileExtensionExtensions
    {
        public static IGameFileExtension WithFiles(this IGame @this)
        {
            return @this.GetExtension<IGameFileExtension>()!;
        }

        public static IGameFileExtensionProvider WithFileLibrary(this IGameLibrary @this)
        {
            return @this.GetExtension<IGameFileExtensionProvider>();
        }
    }
}

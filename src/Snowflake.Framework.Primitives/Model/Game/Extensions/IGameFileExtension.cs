using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Model.FileSystem;
using Snowflake.Model.Records.File;

namespace Snowflake.Model.Game.Extensions
{
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

        IFileRecord RegisterFile(IFile file, string v);

        IDirectory GetSavesLocation(string saveType);

        IDirectory GetRuntimeLocation();
    }

    public static class GameFileExtensionExtensions
    {
        public static IGameFileExtension WithFiles(this IGame @this)
        {
            return @this.GetExtension<IGameFileExtension>()!;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Filesystem;
using Snowflake.Model.Records.File;

namespace Snowflake.Model.Game.LibraryExtensions
{
    /// <summary>
    /// Provides the <see cref="IGameFileExtension"/> for a
    /// <see cref="IGame"/>, and exposes database-level methods that are not
    /// specific to a certain <see cref="IGame"/> instance.
    /// </summary>
    public interface IGameFileExtensionProvider : IGameExtensionProvider<IGameFileExtension>
    {
        /// <summary>
        /// Updates metadata details for the given <see cref="IFileRecord"/>.
        /// </summary>
        /// <param name="file">The file record to update metadata details.</param>
        void UpdateFile(IFileRecord file);

        /// <summary>
        /// Asynchronously updates metadata details for the given <see cref="IFileRecord"/>.
        /// </summary>
        /// <param name="file">The file record to update metadata details.</param>
        Task UpdateFileAsync(IFileRecord file);
    }

    /// <summary>
    /// Provides <see cref="Snowflake.Filesystem"/> managed filesystem access for a <see cref="IGame"/>.
    /// </summary>
    public interface IGameFileExtension : IGameExtension
    {
        /// <summary>
        /// The directory to persist save files for a game.
        /// </summary>
        IDirectory SavesRoot { get; }

        /// <summary>
        /// The directory to store game ROM files and other program data such as assets and game-specific BIOS files.
        /// </summary>
        IDirectory ProgramRoot { get; }

        /// <summary>
        /// The directory to store media such as boxarts, trailers, and screenshots.
        /// </summary>
        IDirectory MediaRoot { get; }

        /// <summary>
        /// The directory to store miscellaneous files related to the game. 
        /// </summary>
        IDirectory MiscRoot { get; }

        /// <summary>
        /// The directory to store resources required for the game to run.
        /// </summary>
        IDirectory ResourceRoot { get; }

        /// <summary>
        /// The directory to store temporary files during the execution of the game.
        /// This directory is cleared before an emulator is run, and the required files
        /// for the emulator can be copied here.
        ///
        /// Afterwards, saves should be copied out from here into <see cref="SavesRoot"/>.
        /// </summary>
        IDirectory RuntimeRoot { get; }

        /// <summary>
        /// Gets the list of <see cref="IFileRecord"/>s that have been associated with this game.
        /// Note that <see cref="IFileRecord"/>s are simply files that have metadata associated with them.
        /// </summary>
        IEnumerable<IFileRecord> GetFileRecords();

        /// <summary>
        /// Asynchronously gets the list of <see cref="IFileRecord"/>s that have been associated with this game.
        /// Note that <see cref="IFileRecord"/>s are simply files that have metadata associated with them.
        /// </summary>
        IAsyncEnumerable<IFileRecord> GetFileRecordsAsync();

        /// <summary>
        /// Gets the <see cref="IFileRecord"/> for a given <see cref="IFile"/> in this game's root directory
        /// if it exists.
        /// </summary>
        /// <param name="file">The <see cref="IFile"/> to get metadata for.</param>
        /// <returns>The <see cref="IFileRecord"/> if it exists, null if it does not.</returns>
        IFileRecord? GetFileInfo(IFile file);

        /// <summary>
        /// Asynchronously gets the <see cref="IFileRecord"/> for a given <see cref="IFile"/> in this game's root directory
        /// if it exists.
        /// </summary>
        /// <param name="file">The <see cref="IFile"/> to get metadata for.</param>
        /// <returns>The <see cref="IFileRecord"/> if it exists, null if it does not.</returns>
        Task<IFileRecord?> GetFileInfoAsync(IFile file);

        /// <summary>
        /// Registers a <see cref="IFile"/> as <see cref="IFileRecord"/>. If this <see cref="IFile"/>
        /// already exists as a <see cref="IFileRecord"/>, the mimetype will be updated with the
        /// new mimetype, but no other existing metadata will be changed.
        /// </summary>
        /// <param name="file">The <see cref="IFile"/> to create a <see cref="IFileRecord"/> for.</param>
        /// <param name="mimetype">The mimetype of the <see cref="IFile"/>, required for <see cref="IFileRecord"/>s</param>
        /// <returns>The <see cref="IFileRecord"/> associated with the provided <see cref="IFile"/>.</returns>
        IFileRecord RegisterFile(IFile file, string mimetype);

        /// <summary>
        /// Asynchronously registers a <see cref="IFile"/> as <see cref="IFileRecord"/>. If this <see cref="IFile"/>
        /// already exists as a <see cref="IFileRecord"/>, the mimetype will be updated with the
        /// new mimetype, but no other existing metadata will be changed.
        /// </summary>
        /// <param name="file">The <see cref="IFile"/> to create a <see cref="IFileRecord"/> for.</param>
        /// <param name="mimetype">The mimetype of the <see cref="IFile"/>, required for <see cref="IFileRecord"/>s</param>
        /// <returns>The <see cref="IFileRecord"/> associated with the provided <see cref="IFile"/>.</returns>
        Task<IFileRecord> RegisterFileAsync(IFile file, string mimetype);

        /// <summary>
        /// Gets a working scratch directory for an emulator instance within <see cref="RuntimeRoot"/>.
        /// </summary>
        /// <returns>A working scratch directory for an emulator running instance. This directory
        /// is guaranteed to be unique. Emulators are responsible for their own cleanup and are well advised
        /// to delete this directory after execution completes.</returns>
        IDirectory GetRuntimeLocation();
    }

    /// <summary>
    /// Fluent extensions to provide access to game configuration access.
    /// </summary>
    public static class GameFileExtensionExtensions
    {
        /// <summary>
        /// Access files for this <see cref="IGame"/>.
        /// </summary>
        /// <param name="this">The <see cref="IGame"/> to access files for.</param>
        /// <returns>An accessor for files for this <see cref="IGame"/>.</returns>
        public static IGameFileExtension WithFiles(this IGame @this)
        {
            return @this.GetExtension<IGameFileExtension>()!;
        }

        /// <summary>
        /// Access files for this <see cref="IGameLibrary"/>.
        /// </summary>
        /// <param name="this">The <see cref="IGameLibrary"/> to access files for.</param>
        /// <returns>An accessor for files for this <see cref="IGame"/>.</returns>
        public static IGameFileExtensionProvider WithFileLibrary(this IGameLibrary @this)
        {
            return @this.GetExtension<IGameFileExtensionProvider>()!;
        }
    }
}

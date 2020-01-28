using Snowflake.Filesystem;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Saving
{
    /// <summary>
    /// Stores and manages <see cref="ISaveGame"/> representations on disk for a given <see cref="IGame"/>.
    /// Each <see cref="ISaveGameManager"/> is scoped to its own <see cref="IGame"/> context, and one
    /// game may contain many, and many types of saves.
    /// </summary>
    public interface ISaveGameManager
    {
        /// <summary>
        /// Creates a new immutable save game.
        /// </summary>
        /// <param name="type">
        /// A unique identifier that identifies all saves of this particular type.
        /// Saves with the same identifier are expected to have the same folder and file structure.
        /// </param>
        /// <param name="factory">A factory function that write the save to the snapshot directory.</param>
        /// <returns>A new <see cref="ISaveGame"/> instance.</returns>
        Task<ISaveGame> CreateSave(string type, Func<IDirectory, Task> factory);
        /// <summary>
        /// Creates a new immutable save game with tags.
        /// </summary>
        /// <param name="type">
        /// A unique identifier that identifies all saves of this particular type.
        /// Saves with the same identifier are expected to have the same folder and file structure.
        /// </param>
        /// <param name="tags">String metadata to identify this save.</param>
        /// <param name="factory">A factory function that write the save to the snapshot directory.</param>
        /// <returns>A new <see cref="ISaveGame"/> instance.</returns>
        Task<ISaveGame> CreateSave(string type, IEnumerable<string> tags, Func<IDirectory, Task> factory);
        /// <summary>
        /// Gets the most recently created save with the given type.
        /// </summary>
        /// <param name="type">
        /// A unique identifier that identifies all saves of this particular type.
        /// Saves with the same identifier are expected to have the same folder and file structure.
        /// </param>
        /// <returns>The most recently created savegame, or null if none were created.</returns>
        ISaveGame? GetLatestSave(string type);
        /// <summary>
        /// Gets the save with the given GUID
        /// </summary>
        /// <param name="guid">The unique GUID of the save.</param>
        /// <returns>The save with the given GUID, or null if it does not exist.</returns>
        ISaveGame? GetSave(Guid guid);
        /// <summary>
        /// Deletes the save with the given GUID
        /// </summary>
        /// <param name="guid">The unique GUID of the save.</param>
        void DeleteSave(Guid guid);
        /// <summary>
        /// Gets all saves registered under this <see cref="ISaveGameManager"/>.
        /// </summary>
        /// <returns>All saves registered under this <see cref="ISaveGameManager"/></returns>
        IEnumerable<ISaveGame> GetSaves();
    }
}
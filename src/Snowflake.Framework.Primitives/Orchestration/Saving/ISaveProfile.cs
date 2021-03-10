using Snowflake.Filesystem;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Saving
{
    /// <summary>
    /// A save profile represents one, or a collection of individual save games that canonically represent the lineage of
    /// a single save instance for one type of save. It may or may not manage a history of save instances, but must be able
    /// to produce one canonical "head" save for a profile regardless of how history is implemented or managed.
    /// </summary>
    public interface ISaveProfile
    {
        /// <summary>
        /// A unique ID used to identify this save profile
        /// </summary>
        Guid Guid { get; }

        /// <summary>
        /// A unique name for the format of the save this save profile produces.
        /// Two profiles with the same save type have the identical file and folder structure
        /// for a save.
        /// </summary>
        string SaveType { get; }

        /// <summary>
        /// The name of this profile.
        /// </summary>
        string ProfileName { get; }

        /// <summary>
        /// The strategy this profile uses to persist saves.
        /// </summary>
        SaveManagementStrategy ManagementStrategy { get; }

        /// <summary>
        /// Creates a new immutable save game. This function should be atomic, and if creating the save fails
        /// should not add a save. The new savegame becomes the "head" save.
        /// </summary>
        /// <param name="saveContents">
        /// The directory of save contents to process in accordance with the <see cref="SaveManagementStrategy"/>.
        /// <para>
        /// For single-file saves in particular, it is important that the save file produced by the emulator
        /// is renamed to "savecontent" to ensure compatibility between emulators that use the same save type.
        /// 
        /// If for some reason this is not desireable, do not use a generic save type such as "sram". If implementing
        /// save-sharing, it is very important to ensure conventions remain identical across all orchestration plugins
        /// that implement the same save type.
        /// </para>
        /// </param>
        /// <returns>A new <see cref="ISaveGame"/> instance.</returns>
        Task<ISaveGame> CreateSave(IReadOnlyDirectory saveContents);

        /// <summary>
        /// Creates a new immutable save game from an existing savegame. This function should be atomic, and if creating the save fails
        /// should not add a save. The new savegame becomes the "head" save.
        /// </summary>
        /// <param name="saveGame">The savegame to create a new save from. The save must be of the same type, 
        /// as this method works by calling <see cref="ISaveGame.ExtractSave(IDirectory)"/></param>
        Task<ISaveGame> CreateSave(ISaveGame saveGame);

        /// <summary>
        /// Gets the "head" save of this profile.
        /// </summary>
        /// <returns>The most recently created savegame, or null if none were created.</returns>
        ISaveGame? GetHeadSave();

        /// <summary>
        /// If supported, returns the saves that make up the history of this profile.
        /// 
        /// This is only supported for <see cref="SaveManagementStrategy.Copy"/> and <see cref="SaveManagementStrategy.Diff"/>,
        /// for all other strategies, this will only return an enumerable of the head save.
        /// </summary>
        /// <returns>All saves registered under this <see cref="ISaveProfile"/></returns>
        IEnumerable<ISaveGame> GetHistory();

        /// <summary>
        /// Clears the history, leaving only the head.
        /// </summary>
        void ClearHistory();
    }
}
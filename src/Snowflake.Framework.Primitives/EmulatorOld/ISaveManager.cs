using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.EmulatorOld
{
    /// <summary>
    /// Provides a save directory path for emulators.
    /// Save paths are unique for each game and are indexed by slot,
    /// except for the shared save directory.
    /// </summary>
    public interface ISaveManager
    {
        /// <summary>
        /// Gets the save directory path for a certain game and slot.
        /// Any negative slot will point to the shared save directory
        /// </summary>
        /// <param name="saveType">The type of save, eg. SRAM, SAV, MCR, WiiNand</param>
        /// <param name="gameGuid">The Guid of the game</param>
        /// <param name="slot">The save slot index</param>
        /// <returns>A path to the save directory given the specified parameters</returns>
        string GetSaveDirectory(string saveType, Guid gameGuid, int slot);

        /// <summary>
        /// Gets the moment the save directory was last accessed using <see cref="GetSaveDirectory"/>.
        /// If the slot does not exist, or is the shared save directory, will return the current time.
        /// </summary>
        /// <param name="saveType">The type of save, eg. SRAM, SAV, MCR, WiiNand</param>
        /// <param name="gameGuid">The Guid of the game</param>
        /// <param name="slot">The save slot index</param>
        /// <returns>The moment last accessed</returns>
        DateTimeOffset GetLastModified(string saveType, Guid gameGuid, int slot);

        /// <summary>
        /// Gets the shared save directory for a save type
        /// </summary>
        /// <param name="saveType">The type of save, eg. SRAM, SAV, MCR, WiiNand</param>
        /// <returns>The shared save directory</returns>
        string GetSharedSaveDirectory(string saveType);

        /// <summary>
        /// Gets the used slot indices for a save type and game.
        /// </summary>
        /// <param name="saveType">The type of save, eg. SRAM, SAV, MCR, WiiNand</param>
        /// <param name="gameGuid">The Guid of the game</param>
        /// <returns>An enumerable of used slot indices for a save type and game.</returns>
        IEnumerable<int> GetUsedSlots(string saveType, Guid gameGuid);
    }
}

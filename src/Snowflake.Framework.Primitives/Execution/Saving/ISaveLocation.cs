using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Snowflake.Execution.Process;

namespace Snowflake.Execution.Saving
{
    /// <summary>
    /// Represents a location where saves from emulators are to persist and
    /// be loaded across EmulatorTasks.
    /// </summary>
    public interface ISaveLocation
    {
        /// <summary>
        /// Gets the GUID of this save location.
        /// </summary>
        Guid LocationGuid { get; }

        /// <summary>
        /// Gets the root directory of this save location.
        /// </summary>
        DirectoryInfo LocationRoot { get; }

        /// <summary>
        /// Gets the game record guid of this save location
        /// </summary>
        Guid RecordGuid { get; }

        /// <summary>
        /// Gets the format of this save file.
        /// Emulators that use the same save format can thus share saves.
        /// </summary>
        string SaveFormat { get; }

        /// <summary>
        /// Gets the date this save location was last persisted to.
        /// </summary>
        DateTimeOffset LastModified { get; }

        /// <summary>
        /// Retrieves the save files from the save location and
        /// copies save files to the emulator save directory.
        /// </summary>
        /// <param name="emulatorSaveDirectory">The directory that the emulator stores saves.</param>
        /// <returns>The files that were copied to the save directory</returns>
        IEnumerable<FileInfo> RetrieveTo(DirectoryInfo emulatorSaveDirectory);

        /// <summary>
        /// Persists all files from the given directory and
        /// copies all files from the emulater save directory to the save location.
        /// Also updates the <see cref="LastModified"/> value.
        /// </summary>
        /// <param name="emulatorSaveDirectory">The </param>
        /// <returns>The files that were copied</returns>
        IEnumerable<FileInfo> PersistFrom(DirectoryInfo emulatorSaveDirectory);
    }
}

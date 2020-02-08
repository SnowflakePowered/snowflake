using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Snowflake.Filesystem;

namespace Snowflake.Orchestration.Saving
{
    /// <summary>
    /// An immutable snapshot of the contents of a save for
    /// a given emulation that stores game resume data (savedata).
    /// 
    /// <see cref="ISaveGame"/> instances are immutable
    /// </summary>
    public interface ISaveGame
    {
        /// <summary>
        /// The timestamp this save was created.
        /// </summary>
        DateTimeOffset CreatedTimestamp { get; }

        /// <summary>
        /// Writes the content of the save to the given directory
        /// </summary>
        Task ExtractSave(IDirectory outputDirectory);

        /// <summary>
        /// A unique identifier that identifies all saves of this particular type.
        /// Saves with the same identifier are expected to have the same folder and file structure.
        /// </summary>
        string SaveType { get; }
    }
}
using System;
using System.Collections.Generic;
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
        /// A unique GUID used to identify this <see cref="ISaveGame"/> within the context of an
        /// <see cref="ISaveGameManager"/>
        /// </summary>
        Guid Guid { get; }
        /// <summary>
        /// The read-only contents of the save.
        /// </summary>
        IReadOnlyDirectory SaveContents { get; }
        /// <summary>
        /// A unique identifier that identifies all saves of this particular type.
        /// Saves with the same identifier are expected to have the same folder and file structure.
        /// </summary>
        string SaveType { get; }
        /// <summary>
        /// Any string tags used to identify this save.
        /// </summary>
        IEnumerable<string> Tags { get; }
    }
}
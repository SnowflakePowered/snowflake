using System;
using Snowflake.Information;
using Snowflake.Information.MediaStore;
namespace Snowflake.Game
{
    /// <summary>
    /// Represents a game with all it's information
    /// </summary>
    public interface IGameInfo : IInfo
    {
        /// <summary>
        /// The CRC32 of the game
        /// </summary>
        string CRC32 { get; }
        /// <summary>
        /// The path to the game's ROM
        /// </summary>
        string FileName { get; }
        /// <summary>
        /// The unique ID of the game
        /// </summary>
        string UUID { get; }
        ///<summary>
        ///If the mediastore exists, returns the mediastore.
        ///The key to access the mediastore is accessed under snowflake_mediastore in the game metadata
        ///</summary>
        IMediaStore MediaStore { get; }
    }
}

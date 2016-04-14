using System.Collections.Generic;

namespace Snowflake.Game
{
    /// <summary>
    /// Represents a game with all it's information
    /// </summary>
    public interface IGameInfo 
    {
        /// <summary>
        /// The id of the platform that this object is related to
        /// </summary>
        string PlatformID { get; }
        /// <summary>
        /// The title of this game
        /// </summary>
        string Title { get; }
        /// <summary>
        /// Any metadata that is attached to this object, such as descriptions
        /// </summary>
        IDictionary<string, string> Metadata { get; set; }
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
    }
}

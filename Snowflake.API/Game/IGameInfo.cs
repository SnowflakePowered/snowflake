using System;
using Snowflake.Information;
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
    }
}

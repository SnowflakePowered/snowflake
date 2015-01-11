using System;
using Snowflake.Game;
namespace Snowflake.Emulator.Configuration
{
    /// <summary>
    /// A place to store configuration profiles for a certain configuration template
    /// </summary>
    public interface IConfigurationStore
    {
        /// <summary>
        /// The path to the configuration store
        /// </summary>
        string ConfigurationStorePath { get; }
        /// <summary>
        /// Whether the store contains a profile for a certain game
        /// </summary>
        /// <param name="gameInfo">The game it contains</param>
        /// <returns>True if the profile exists, false if not</returns>
        bool Contains(IGameInfo gameInfo);
        /// <summary>
        /// Whether the store contains a profile for a rom with a certain CRC32
        /// </summary>
        /// <param name="gameInfo">The game to search for</param>
        /// <returns>True if the profile exists, false if not</returns>
        bool ContainsCRC32(IGameInfo gameInfo);
        /// <summary>
        /// Whether the store contains a profile for a rom with a certain filename
        /// </summary>
        /// <param name="gameInfo">The game to search for</param>
        /// <returns>True if the profile exists, false if not</returns>
        bool ContainsFilename(IGameInfo gameInfo);
        /// <summary>
        /// The default configuration profile
        /// </summary>
        IConfigurationProfile DefaultProfile { get; }
        /// <summary>
        /// Gets the configuration profile for a certain game. 
        /// If none exists specifically for that game the default profile is returned
        /// </summary>
        /// <param name="gameInfo">The game to get configuration for</param>
        /// <returns>The configuration of the game</returns>
        IConfigurationProfile GetConfigurationProfile(IGameInfo gameInfo);
        /// <summary>
        /// The template ID this configuration store is for
        /// </summary>
        string TemplateID { get; }
        /// <summary>
        /// An indexer alias to GetConfigurationProfile
        /// </summary>
        /// <param name="gameInfo">The game to get configuration for</param>
        /// <returns>The configuration of the game</returns>
        IConfigurationProfile this[IGameInfo gameInfo] { get; }
    }
}

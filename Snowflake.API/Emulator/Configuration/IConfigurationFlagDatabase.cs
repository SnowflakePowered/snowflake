using System;
using System.Collections.Generic;
using Snowflake.Game;
namespace Snowflake.Emulator.Configuration
{
    /// <summary>
    /// The Configuration Flag Database holds the flag data for a certain game
    /// Every emulator bridge gets it's own flag table and the flags are accessible by emulator ID and gameInfo.
    /// </summary>
    public interface IConfigurationFlagDatabase
    {
        /// <summary>
        /// Add a game to the Configuration Flag Database
        /// </summary>
        /// <param name="gameInfo">The game to add</param>
        /// <param name="emulatorId">The emulator ID of the flagset Emulator Bridge</param>
        /// <param name="configFlags">The flagset of the Emulator Bridge</param>
        /// <param name="flagValues">The values to be added</param>
        void AddGame(IGameInfo gameInfo, string emulatorId, IList<IConfigurationFlag> configFlags, IDictionary<string, string> flagValues);
        /// <summary>
        /// Create a table for the corresponding emulator with the specified flagset
        /// </summary>
        /// <param name="emulatorId">The emulator ID to create a table for</param>
        /// <param name="configFlags">The flagset of the emulator</param>
        void CreateFlagsTable(string emulatorId, IList<IConfigurationFlag> configFlags);
        /// <summary>
        /// Get a configuration flag value from the database
        /// </summary>
        /// <param name="gameInfo">The game to access for</param>
        /// <param name="emulatorId">The ID of the emulator</param>
        /// <param name="key">The flag key to access</param>
        /// <param name="type">The type of flag it is</param>
        /// <returns>An object that can be casted to either bool, int or string depending on flag type</returns>
        object GetValue(IGameInfo gameInfo, string emulatorId, string key, ConfigurationFlagTypes type);
    }
}

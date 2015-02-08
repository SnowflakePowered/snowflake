using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Game;
namespace Snowflake.Emulator.Configuration
{
    public interface IConfigurationFlagStore
    {
        /// <summary>
        /// Adds a game to the store with the specified flag values
        /// </summary>
        /// <param name="gameInfo"></param>
        /// <param name="flagValues"></param>
        void AddGame(IGameInfo gameInfo, IDictionary<string, string> flagValues);
        /// <summary>
        /// Adds a game to the store with no values
        /// </summary>
        /// <param name="gameInfo"></param>
        void AddGame(IGameInfo gameInfo);
        /// <summary>
        /// Get a configuration flag value from the store
        /// </summary>
        /// <param name="gameInfo">The game to access for</param>
        /// <param name="key">The flag key to access</param>
        /// <param name="type">The type of flag it is</param>
        /// <returns>An object that can be casted to either bool, int or string depending on flag type</returns>
        dynamic GetValue(IGameInfo gameInfo, string key, ConfigurationFlagTypes type);
        /// <summary>
        /// Sets a value to the store
        /// </summary>
        /// <param name="gameInfo">The game to access for</param>
        /// <param name="key">The key to set the value of</param>
        /// <param name="value">The value to set</param>
        /// <param name="type">The type of flag it is</param>
        void SetValue(IGameInfo gameInfo, string key, dynamic value, ConfigurationFlagTypes type);
        /// <summary>
        /// Get the default configuration flag value from the store
        /// </summary>
        /// <param name="gameInfo">The game to access for</param>
        /// <param name="key">The flag key to access</param>
        /// <param name="type">The type of flag it is</param>
        /// <returns>An object that can be casted to either bool, int or string depending on flag type</returns>
        dynamic GetDefaultValue(string key, ConfigurationFlagTypes type);
        /// <summary>
        /// Sets the a value to the store
        /// </summary>
        /// <param name="gameInfo">The game to access for</param>
        /// <param name="key">The key to set the value of</param>
        /// <param name="value">The value to set</param>
        /// <param name="type">The type of flag it is</param>
        void SetDefaultValue(string key, dynamic value, ConfigurationFlagTypes type);
        /// <summary>
        /// The corresponding emulator bridge ID for this store.
        /// </summary>
        string EmulatorBridgeID { get; }
        /// <summary>
        /// Indexer shim for GetValue and SetValue
        /// </summary>
        /// <param name="gameInfo">The game to access</param>
        /// <param name="key">The key to access</param>
        /// <param name="type">The type of flag</param>
        /// <returns>An object that can be casted to either bool, int or string depending on flag type</returns>
        dynamic this[IGameInfo gameInfo, string key, ConfigurationFlagTypes type] { get; set; }
    }
}

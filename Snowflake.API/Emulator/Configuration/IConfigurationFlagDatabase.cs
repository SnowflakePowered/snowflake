using System;
using System.Collections.Generic;
using Snowflake.Game;
namespace Snowflake.Emulator.Configuration
{
    public interface IConfigurationFlagDatabase
    {
        void AddGame(IGameInfo gameInfo, string emulatorId, System.Collections.Generic.IList<IConfigurationFlag> configFlags, IDictionary<string, string> flagValues);
        void CreateFlagsTable(string emulatorId, IList<IConfigurationFlag> configFlags);
        object GetValue(IGameInfo gameInfo, string emulatorId, string key, ConfigurationFlagTypes type);
    }
}

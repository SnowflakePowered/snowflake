using System;
using Snowflake.Game;
namespace Snowflake.Emulator.Configuration
{
    public interface IConfigurationStore
    {
        string ConfigurationStorePath { get; }
        bool Contains(IGameInfo gameInfo);
        bool ContainsCRC32(IGameInfo gameInfo);
        bool ContainsFilename(IGameInfo gameInfo);
        IConfigurationProfile DefaultProfile { get; }
        IConfigurationProfile GetConfigurationProfile(IGameInfo gameInfo);
        string TemplateID { get; }
        IConfigurationProfile this[IGameInfo gameInfo] { get; }
    }
}

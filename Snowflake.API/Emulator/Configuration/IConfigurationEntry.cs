using System;
namespace Snowflake.Emulator.Configuration
{
    public interface IConfigurationEntry
    {
        dynamic DefaultValue { get; }
        string Description { get; }
        string Name { get; }
    }
}

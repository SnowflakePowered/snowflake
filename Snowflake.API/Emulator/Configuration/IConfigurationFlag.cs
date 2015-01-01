using System;
namespace Snowflake.Emulator.Configuration
{
    public interface IConfigurationFlag
    {
        string DefaultValue { get; }
        string Description { get; }
        string Key { get; }
        System.Collections.Generic.IReadOnlyDictionary<string, string> SelectValues { get; }
        ConfigurationFlagTypes Type { get; }
    }
}

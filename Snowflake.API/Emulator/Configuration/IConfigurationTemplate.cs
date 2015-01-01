using System;
using System.Collections.Generic;
namespace Snowflake.Emulator.Configuration
{
    public interface IConfigurationTemplate
    {
        IBooleanMapping BooleanMapping { get; }
        IList<IConfigurationEntry> ConfigurationEntries { get; }
        string ConfigurationName { get; }
        string FileName { get; }
        string StringTemplate { get; set; }
    }
}

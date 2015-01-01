using System;
using System.Collections.Generic;
namespace Snowflake.Emulator.Configuration
{
    public interface IConfigurationProfile
    {
        IReadOnlyDictionary<string, dynamic> ConfigurationValues { get; }
        string TemplateID { get; }
    }
}

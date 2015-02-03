using System;
using System.Collections.Generic;
namespace Snowflake.Emulator.Configuration
{
    /// <summary>
    /// The configuration profile represents a serializable file that maps the values of an emulator configuration to 
    /// a configuration template where they are combined upon compilation and an emulator configuration file is produced.
    /// <see cref="Snowflake.Emulator.Configuration.IConfigurationTemplate"/> for how configuration templates work
    /// </summary>
    public interface IConfigurationProfile
    {
        /// <summary>
        /// The values of the configuration where the key is the name of the configuration value as specified by the configuration template
        /// </summary>
        IDictionary<string, dynamic> ConfigurationValues { get; }
        /// <summary>
        /// The ID of the configuration template this profile corresponds to 
        /// </summary>
        string TemplateID { get; }
    }
}

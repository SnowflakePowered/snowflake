using System;
using System.Collections.Generic;
namespace Snowflake.Emulator.Configuration
{
    /// <summary>
    /// A configuration template abstracts the configuration of an emulator and represents it as a standard specification
    /// It provides all that is nescessary to produce a valid emulator configuration given the values it requires in the form of
    /// an <see cref="Snowflake.Emulator.Configuration.IConfigurationProfile"/>
    /// </summary>
    public interface IConfigurationTemplate
    {
        /// <summary>
        /// The boolean mapping for this configuration
        /// </summary>
        IBooleanMapping BooleanMapping { get; }
        /// <summary>
        /// The list of valid configuration entries 
        /// </summary>
        IList<IConfigurationEntry> ConfigurationEntries { get; }
        /// <summary>
        /// The name of the configuration, for use in identification during compilation
        /// </summary>
        string ConfigurationName { get; }
        /// <summary>
        /// The default filename of the configuration
        /// </summary>
        string FileName { get; }
        /// <summary>
        /// The string template to replace with sufficient values to generate a valid configuration file
        /// </summary>
        string StringTemplate { get; set; }
    }
}

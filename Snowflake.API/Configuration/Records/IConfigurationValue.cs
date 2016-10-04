using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records;

namespace Snowflake.Configuration.Records
{
    /// <summary>
    /// Represents a configuration value record for a game.
    /// The configuration value record should be a version 3 GUID namespaced on 
    /// the record GUID, based on the option name concatenated with the configuration file name in the format
    /// {ConfigurationFileName}::{OptionName}
    /// where ConfigurationFileName should be the name of the 
    /// For a configuration value that is not related to any game (orphaned configuration), 
    /// the GUID should be namespaced on b1c873f8-e9c6-4c59-bc3b-8da869ad99d3
    /// </summary>
    public interface IConfigurationValue : IRecord
    {
        /// <summary>
        /// The value of the configuration option.
        /// </summary>
        object Value { get; set; }
        /// <summary>
        /// The type of the configuration option. Must match object value.
        /// </summary>
        Type Type { get; }
        /// <summary>
        /// The game record attached to this configuration value.
        /// </summary>
        Guid Record { get; }
        /// <summary>
        /// The section guid attached to this configuration value
        /// </summary>
        Guid Section { get; }
    }
}

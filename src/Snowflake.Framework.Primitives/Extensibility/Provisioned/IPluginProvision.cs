using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Snowflake.Configuration;
using Snowflake.Extensibility.Configuration;

namespace Snowflake.Extensibility.Provisioned
{
    /// <summary>
    /// The plugin provisions provided by the plugin manager
    /// </summary>
    public interface IPluginProvision
    {
        /// <summary>
        /// Gets the logger for the plugin
        /// </summary>
        ILogger Logger { get; }

        /// <summary>
        /// Gets the plugin's properties
        /// </summary>
        IPluginProperties Properties { get; }

        /// <summary>
        /// Gets the plugin's configuration store
        /// </summary>
        IPluginConfigurationStore ConfigurationStore { get; }

        /// <summary>
        /// Gets the plugin's name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the author of the plugin.
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Gets a short description of the plugin
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the version of the plugin
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// Gets this plugin's content directory
        /// </summary>
        DirectoryInfo ContentDirectory { get; }

        /// <summary>
        /// Gets the plugin's resource directory
        /// </summary>
        DirectoryInfo ResourceDirectory { get; }

        /// <summary>
        /// Gets the resource directory common to the plugin's module.
        /// </summary>
        DirectoryInfo CommonResourceDirectory { get; }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Extensibility
{
    /// <summary>
    /// The plugin provisions provided by the plugin manager
    /// </summary>
    public interface IPluginProvision
    {
        /// <summary>
        /// The logger for the plugin
        /// </summary>
        ILogger Logger { get; }

        /// <summary>
        /// The plugin's properties
        /// </summary>
        IPluginProperties Properties { get; }

        /// <summary>
        /// The plugin's name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The author of the plugin.
        /// </summary>
        string Author { get; }

        /// <summary>
        /// A short description of the plugin
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The version of the plugin
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// This plugin's content directory
        /// </summary>
        DirectoryInfo ContentDirectory { get; }

        /// <summary>
        /// The plugin's resource directory
        /// </summary>
        DirectoryInfo ResourceDirectory { get; }

        /// <summary>
        /// The resource directory common to the plugin's module.
        /// </summary>
        DirectoryInfo CommonResourceDirectory { get; }
    }
}

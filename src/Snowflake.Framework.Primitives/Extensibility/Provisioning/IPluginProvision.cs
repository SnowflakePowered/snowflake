using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Snowflake.Configuration;
using Snowflake.Extensibility.Configuration;
using Snowflake.Filesystem;

namespace Snowflake.Extensibility.Provisioning
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
        /// Gets this plugin's content directory, or the root location of the plugin.
        /// 
        /// This should only be used for discretionary or informational purposes. Consider
        /// using <see cref="IPluginProvision.DataDirectory"/>, <see cref="IPluginProvision.ResourceDirectory"/>,
        /// or  <see cref="IPluginProvision.CommonResourceDirectory"/> instead.
        /// </summary>
        DirectoryInfo ContentDirectory { get; }

        /// <summary>
        /// Gets the plugin's data directory. The data directory should be used for temporary or
        /// generated data that may change or be updated frequently, such as auxillary configurations files
        /// or temporary files.
        /// 
        /// For configuration specifically, consider using <see cref="IPluginProvision.ConfigurationStore"/>.
        /// </summary>
        IDirectory DataDirectory { get; }

        /// <summary>
        /// Gets the plugin's resource directory. The resource directory is intended for static
        /// files that are required only by this plugin, and should not be modified after install.
        /// </summary>
        IDirectory ResourceDirectory { get; }

        /// <summary>
        /// Gets the resource directory common to the plugin's module. The common resource directory is intended for static
        /// files that are shared between all plugins in the current module, and should not be modified after install.
        /// </summary>
        IDirectory CommonResourceDirectory { get; }
    }
}

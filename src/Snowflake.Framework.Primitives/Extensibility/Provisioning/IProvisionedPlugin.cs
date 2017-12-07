using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Configuration;

namespace Snowflake.Extensibility.Provisioning
{
    /// <summary>
    /// Represents a plugin initialized with a provision
    /// A provisioned plugin has access to a logger, configuration options, and a content directory.
    /// </summary>
    public interface IProvisionedPlugin : IPlugin
    {
        /// <summary>
        /// Gets the plugin provision from the active plugin manager for this instance
        /// </summary>
        IPluginProvision Provision { get; }

        /// <summary>
        /// Gets the plugin configuration
        /// </summary>
        /// <returns>The plugin configuration</returns>
        IConfigurationSection GetPluginConfiguration();
    }
}

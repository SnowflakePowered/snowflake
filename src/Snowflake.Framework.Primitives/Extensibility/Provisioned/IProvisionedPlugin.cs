using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Configuration;

namespace Snowflake.Extensibility.Provisioned
{
    public interface IProvisionedPlugin : IPlugin
    {
        /// <summary>
        /// The plugin provision from the active plugin manager for this instance
        /// </summary>
        IPluginProvision Provision { get; }

        /// <summary>
        /// Gets the plugin configuration
        /// </summary>
        /// <returns>The plugin configuration</returns>
        IConfigurationSection GetPluginConfiguration();

    }
}

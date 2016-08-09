using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Snowflake.Extensibility.Configuration;
using Snowflake.Service;

namespace Snowflake.Extensibility
{
    /// <summary>
    /// The common interface between all plugins. 
    /// Only classes that are derived from IPlugin will be imported
    /// </summary>
    public interface IPlugin : IDisposable
    {
        /// <summary>
        /// The name of the plugin
        /// </summary>
        string PluginName { get; }
        /// <summary>
        /// The path where the plugin is able to store files and data within the plugin subdirectory
        /// </summary>
        string PluginDataPath { get; }
        /// <summary>
        /// A dictionary containing the information within plugin.json
        /// </summary>
        IPluginProperties PluginProperties { get; }
        /// <summary>
        /// The IPluginConfiguration configuration associated with this plugin.
        /// This is null unless it has been initialized by the plugin.
        /// </summary>
        IPluginConfiguration PluginConfiguration { get; }
        /// <summary>
        /// A list of options exposed to the frontend. This writes to PluginConfiguration on save.
        /// </summary>
        IList<IPluginConfigOption> PluginConfigurationOptions { get; }
    }
}

﻿using System;
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
        /// The list of platforms a plugin supports if applicable. If not, leave this empty
        /// </summary>
        IList<string> SupportedPlatforms { get; }
        /// <summary>
        /// The Assembly object representation of the compiled plugin
        /// </summary>
        Assembly PluginAssembly { get; }
        /// <summary>
        /// A dictionary containing the information within plugin.json
        /// </summary>
        IDictionary<string, dynamic> PluginInfo { get; }
        /// <summary>
        /// A reference to the current running ICoreService instance. 
        /// This is import-injected upon composition in the plugin constructor.
        /// </summary>
        ICoreService CoreInstance { get; }
        /// <summary>
        /// Gets an embedded resource as a Stream from the plugin Assembly.
        /// Wraps GetManifestResourceStream so that specifiying the full namespace of the resource is not required
        /// </summary>
        /// <param name="resourceName">The name of the resource</param>
        /// <returns>The resource as a stream</returns>
        Stream GetResource(string resourceName);
        /// <summary>
        /// Gets an embedded resource as a String from the plugin Assembly
        /// </summary>
        /// <param name="resourceName">The name of the resource</param>
        /// <returns>The resource as a string</returns>
        string GetStringResource(string resourceName);
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

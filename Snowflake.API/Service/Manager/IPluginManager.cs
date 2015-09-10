using System;
using System.Collections.Generic;
using Snowflake.Emulator;
using Snowflake.Identifier;
using Snowflake.Plugin;
using Snowflake.Scraper;

namespace Snowflake.Service.Manager
{
    /// <summary>
    /// The IPluginManager manages all plugins except for Ajax plugins
    /// </summary>
    public interface IPluginManager : IDisposable
    {
        /// <summary>
        /// The location from which plugins are loaded
        /// </summary>
        string LoadablesLocation { get; }
        /// <summary>
        /// A list of loaded plugins by plugin name
        /// </summary>
        IReadOnlyDictionary<string, Type> Registry { get; }
        /// <summary>
        /// Add a loadable type to register
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void AddType<T>() where T : IBasePlugin;
        /// <summary>
        /// Initialize the plugin manager by loading all plugins
        /// </summary>
        void Initialize();
        /// <summary>
        /// Get a dictionary of plugins by IBasePlugin type
        /// </summary>
        /// <typeparam name="T">Type to get</typeparam>
        /// <returns></returns>
        IDictionary<string, T> Plugins<T>() where T : IBasePlugin;
        /// <summary>
        /// Get a plugin by plugin name and expected type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pluginName"></param>
        /// <returns>The plugin</returns>
        T Plugin<T>(string pluginName) where T : IBasePlugin;
        /// <summary>
        /// Whethere or not the plugin manager has been initialized
        /// </summary>
        bool IsInitialized { get; }
    }
}

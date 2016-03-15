using System;
using System.Collections.Generic;
using Snowflake.Extensibility;


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
        /// Initialize the plugin manager by loading all plugins
        /// </summary>
        void Initialize();
        /// <summary>
        /// Get a dictionary of plugins by IPlugin type
        /// </summary>
        /// <typeparam name="T">Type to get</typeparam>
        /// <returns></returns>
        IDictionary<string, T> Get<T>() where T : IPlugin;
        /// <summary>
        /// Get a plugin by plugin name and expected type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pluginName"></param>
        /// <returns>The plugin</returns>
        T Get<T>(string pluginName) where T : IPlugin;
        /// <summary>
        /// Registers an object as a plugin
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="plugin"></param>
        void Register<T>(T plugin) where T : IPlugin;
        /// <summary>
        /// Whethere or not the plugin manager has been initialized
        /// </summary>
        bool IsInitialized { get; }
    }
}

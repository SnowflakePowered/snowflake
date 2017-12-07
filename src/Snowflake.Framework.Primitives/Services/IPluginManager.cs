using System;
using System.Collections.Generic;
using Snowflake.Emulator;
using Snowflake.Extensibility;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Loader;

namespace Snowflake.Services
{
    /// <summary>
    /// The IPluginManager manages all plugins except for Ajax plugins
    /// </summary>
    public interface IPluginManager : IDisposable
    {
        /// <summary>
        /// Provisions a <see cref="IProvisionedPlugin"/> relative to the given module for initialization.
        /// </summary>
        /// <typeparam name="T">The Type of the plugin</typeparam>
        /// <param name="module">The module the plugin is loaded from</param>
        /// <returns>A provision used to initialize an <see cref="IProvisionedPlugin"/></returns>
        IPluginProvision GetProvision<T>(IModule module)
            where T : IPlugin;

        /// <summary>
        /// Registers a plugin with the plugin manager.
        /// </summary>
        /// <typeparam name="T">
        /// The plugin type category to register under.
        /// Examples include <see cref="IEmulatorAdapter"/>
        /// </typeparam>
        /// <param name="plugin">The plugin instance</param>
        void Register<T>(T plugin)
            where T : IPlugin;

        /// <summary>
        /// Gets all plugins registered under the type category
        /// </summary>
        /// <typeparam name="T">
        /// The plugin type category to register under.
        /// Examples include <see cref="IEmulatorAdapter"/>
        /// </typeparam>
        /// <returns>All plugins registered under a specific category.</returns>
        IEnumerable<T> Get<T>()
            where T : IPlugin;

        /// <summary>
        /// Gets a specific plugin registered under a given type category
        /// </summary>
        /// <typeparam name="T">
        /// The plugin type category to register under.
        /// Examples include <see cref="IEmulatorAdapter"/>
        /// </typeparam>
        /// <param name="pluginName">The name of the plugin.</param>
        /// <returns>The given plugin if it exists, null if it does not.</returns>
        T Get<T>(string pluginName)
            where T : IPlugin;

        /// <summary>
        /// Gets a specific provisioned plugin
        /// </summary>
        /// <param name="pluginName">The name of the plugin.</param>
        /// <returns>The given plugin if it exists, null if it does not.</returns>
        IProvisionedPlugin Get(string pluginName);

        /// <summary>
        /// Checks if a given plugin under a type category has been loaded into the plugin manager.
        /// </summary>
        /// <typeparam name="T">
        /// The plugin type category to register under.
        /// Examples include <see cref="IEmulatorAdapter"/>
        /// </typeparam>
        /// <param name="pluginName">The name of the plugin.</param>
        /// <returns>True if the plugin has been registered.</returns>
        bool IsRegistered<T>(string pluginName)
            where T : IPlugin;

        /// <summary>
        /// Checks if a given plugin has been loaded into the plugin manager.
        /// </summary>
        /// <param name="pluginName">The name of the plugin.</param>
        /// <returns>True if the plugin has been registered.</returns>
        bool IsRegistered(string pluginName);
    }
}

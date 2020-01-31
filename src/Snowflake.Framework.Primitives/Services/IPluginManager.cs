﻿using System;
using System.Collections.Generic;
using Snowflake.Extensibility;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Loader;

namespace Snowflake.Services
{
    /// <summary>
    /// The IPluginManager manages all plugins.
    /// </summary>
    public interface IPluginManager : IDisposable, IEnumerable<IPlugin>
    {
        /// <summary>
        /// Provisions a <see cref="IProvisionedPlugin"/> relative to the given module for initialization.
        /// </summary>
        /// <typeparam name="T">The Type of the plugin</typeparam>
        /// <param name="module">The module the plugin is loaded from</param>
        /// <returns>A provision used to initialize an <see cref="IProvisionedPlugin"/></returns>
        IPluginProvision GetProvision<T>(IModule module)
            where T : class, IPlugin;

        /// <summary>
        /// Registers a plugin with the plugin manager.
        /// </summary>
        /// <typeparam name="T">
        /// The plugin type category to register under.
        /// </typeparam>
        /// <param name="plugin">The plugin instance</param>
        void Register<T>(T plugin)
            where T : class, IPlugin;

        /// <summary>
        /// Gets all plugins registered under the type category
        /// </summary>
        /// <typeparam name="T">
        /// The plugin type category to register under.
        /// </typeparam>
        /// <returns>All plugins registered under a specific category.</returns>
        IEnumerable<T> Get<T>()
            where T : class, IPlugin;

        /// <summary>
        /// Gets all plugins registered under the type category as a plugin collection.
        /// </summary>
        /// <typeparam name="T">
        /// The plugin type category to register under.
        /// </typeparam>
        /// <returns>All plugins registered under a specific category.</returns>
        IPluginCollection<T> GetCollection<T>()
            where T : class, IPlugin;

        /// <summary>
        /// Gets a specific plugin registered under a given type category
        /// </summary>
        /// <typeparam name="T">
        /// The plugin type category to register under.
        /// </typeparam>
        /// <param name="pluginName">The name of the plugin.</param>
        /// <returns>The given plugin if it exists, null if it does not.</returns>
        T? Get<T>(string pluginName)
            where T : class, IPlugin;

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
        /// </typeparam>
        /// <param name="pluginName">The name of the plugin.</param>
        /// <returns>True if the plugin has been registered.</returns>
        bool IsRegistered<T>(string pluginName)
            where T : class, IPlugin;

        /// <summary>
        /// Checks if a given plugin has been loaded into the plugin manager.
        /// </summary>
        /// <param name="pluginName">The name of the plugin.</param>
        /// <returns>True if the plugin has been registered.</returns>
        bool IsRegistered(string pluginName);
    }
}

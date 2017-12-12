using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Services;

namespace Snowflake.Extensibility
{
    /// <summary>
    /// Represents an always-updated collection of plugins relative to
    /// a <see cref="IPluginManager"/> that always gets all the plugins loaded of type T.
    /// </summary>
    /// <typeparam name="T">The type of plugin</typeparam>
    public interface IPluginCollection<T> : IEnumerable<T>
        where T : IPlugin
    {
        /// <summary>
        /// Gets the instance of T.
        /// </summary>
        /// <param name="pluginName">The name of the plugin</param>
        /// <returns>An instance of the plugin with the given plugin name. </returns>
        T this[string pluginName] { get; }
    }
}

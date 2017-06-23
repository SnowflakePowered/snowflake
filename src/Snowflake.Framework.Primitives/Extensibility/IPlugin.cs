using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Snowflake.Extensibility.Configuration;
using Snowflake.Services;

namespace Snowflake.Extensibility
{
    /// <summary>
    /// The common interface between all plugins. 
    /// </summary>
    public interface IPlugin : IDisposable
    {
        /// <summary>
        /// The name of the plugin
        /// </summary>
        string Name { get; }
        /// <summary>
        /// The author of the plugin.
        /// </summary>
        string Author { get; }
        /// <summary>
        /// A short description of the plugin
        /// </summary>
        string Description { get; }
        /// <summary>
        /// The version of the plugin
        /// </summary>
        Version Version { get; }
    }
}

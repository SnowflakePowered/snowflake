using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Snowflake.Configuration;
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
        /// Gets the name of the plugin
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the author of the plugin.
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Gets a short description of the plugin
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the version of the plugin
        /// </summary>
        Version Version { get; }
    }
}

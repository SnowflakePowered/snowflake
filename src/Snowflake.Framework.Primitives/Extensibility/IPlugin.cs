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
        string PluginName { get; }
        /// <summary>
        /// The plugin provision from the active plugin manager for this instance
        /// </summary>
        IPluginProvision Provision { get; }
    }
}

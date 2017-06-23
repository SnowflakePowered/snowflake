using System;
using System.Collections.Generic;
using Snowflake.Extensibility;
using Snowflake.Loader;

namespace Snowflake.Services
{
    /// <summary>
    /// The IPluginManager manages all plugins except for Ajax plugins
    /// </summary>
    public interface IPluginManager : IDisposable
    {
        IPluginProvision GetProvision<T>(IModule module) where T: IPlugin;
        void Register<T>(T plugin) where T : IPlugin;
    }
}

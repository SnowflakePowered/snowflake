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
    public interface IPluginManager : ILoadableManager, IDisposable
    {
        /// <summary>
        /// The loaded emulator plugins
        /// </summary>
        IReadOnlyDictionary<string, IEmulatorBridge> LoadedEmulators { get; }
        /// <summary>
        /// The loaded identifier plugins
        /// </summary>
        IReadOnlyDictionary<string, IIdentifier> LoadedIdentifiers { get; }
        /// <summary>
        /// The loaded general plugins
        /// </summary>
        IReadOnlyDictionary<string, IGeneralPlugin> LoadedPlugins { get; }
        /// <summary>
        /// The loaded scraper plugins
        /// </summary>
        IReadOnlyDictionary<string, IScraper> LoadedScrapers { get; }
    }
}

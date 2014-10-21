using System;
using System.Collections.Generic;
using Snowflake.Emulator;
using Snowflake.Plugin;
using Snowflake.Scraper;

namespace Snowflake.Core.Manager.Interface
{
    public interface IPluginManager : ILoadableManager
    {
        IDictionary<string, IEmulator> LoadedEmulators { get; }
        IDictionary<string, IIdentifier> LoadedIdentifiers { get; }
        IDictionary<string, IGenericPlugin> LoadedPlugins { get; }
        IDictionary<string, IScraper> LoadedScrapers { get; }
    }
}

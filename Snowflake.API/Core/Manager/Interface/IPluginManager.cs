using System;
using System.Collections.Generic;
using Snowflake.Emulator;
using Snowflake.Plugin;
using Snowflake.Scraper;

namespace Snowflake.Core.Manager.Interface
{
    public interface IPluginManager : ILoadableManager
    {
        IReadOnlyDictionary<string, IEmulator> LoadedEmulators { get; }
        IReadOnlyDictionary<string, IIdentifier> LoadedIdentifiers { get; }
        IReadOnlyDictionary<string, IGenericPlugin> LoadedPlugins { get; }
        IReadOnlyDictionary<string, IScraper> LoadedScrapers { get; }
    }
}

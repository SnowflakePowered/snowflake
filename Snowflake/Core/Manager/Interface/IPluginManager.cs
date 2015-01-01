using System;
using System.Collections.Generic;
using Snowflake.Emulator;
using Snowflake.Plugin;
using Snowflake.Scraper;
using Snowflake.Identifier;

namespace Snowflake.Service.Manager.Interface
{
    public interface IPluginManager : ILoadableManager
    {
        IReadOnlyDictionary<string, IEmulatorBridge> LoadedEmulators { get; }
        IReadOnlyDictionary<string, IIdentifier> LoadedIdentifiers { get; }
        IReadOnlyDictionary<string, IGeneralPlugin> LoadedPlugins { get; }
        IReadOnlyDictionary<string, IScraper> LoadedScrapers { get; }
    }
}

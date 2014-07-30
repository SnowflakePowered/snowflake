using System;
using System.Collections.Generic;
using Snowflake.API.Interface.Plugin;
namespace Snowflake.API.Interface.Core
{
    public interface IPluginManager
    {
        void LoadAllPlugins();
        IDictionary<string, IEmulator> LoadedEmulators { get; }
        IDictionary<string, IIdentifier> LoadedIdentifiers { get; }
        IDictionary<string, IGenericPlugin> LoadedPlugins { get; }
        IDictionary<string, IScraper> LoadedScrapers { get; }
        IDictionary<string, Type> PluginRegistry { get; }
    }
}

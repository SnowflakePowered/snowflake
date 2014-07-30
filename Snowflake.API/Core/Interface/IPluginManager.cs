using System;
using System.Collections.Generic;
using Snowflake.Plugin.Interface;
namespace Snowflake.Core.Interface
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

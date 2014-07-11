using System;
using System.Collections.Generic;
namespace Snowflake.API.Interface.Core
{
    public interface IPluginManager
    {
        void LoadAllPlugins();
        IDictionary<string, Snowflake.API.Interface.IEmulator> LoadedEmulators { get; }
        IDictionary<string, Snowflake.API.Interface.IIdentifier> LoadedIdentifiers { get; }
        IDictionary<string, Snowflake.API.Interface.IGenericPlugin> LoadedPlugins { get; }
        IDictionary<string, Snowflake.API.Interface.IScraper> LoadedScrapers { get; }
        IDictionary<string, Type> PluginRegistry { get; }
    }
}

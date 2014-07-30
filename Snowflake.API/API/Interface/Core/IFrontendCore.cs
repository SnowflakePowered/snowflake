using System;
using Snowflake.API.Database;
using System.Collections.Generic;
namespace Snowflake.API.Interface.Core
{
    public interface IFrontendCore
    {
        string AppDataDirectory { get; }
        event EventHandler CoreLoaded;
        GameDatabase GameDatabase { get; }
        IDictionary<string, global::Snowflake.API.Information.Platform.Platform> LoadedPlatforms { get; }
        IPluginManager PluginManager { get; }
    }
}

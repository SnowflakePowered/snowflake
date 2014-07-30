using System;
using Snowflake.Database;
using System.Collections.Generic;
namespace Snowflake.Core.Interface
{
    public interface IFrontendCore
    {
        string AppDataDirectory { get; }
        event EventHandler CoreLoaded;
        GameDatabase GameDatabase { get; }
        IDictionary<string, global::Snowflake.Information.Platform.Platform> LoadedPlatforms { get; }
        IPluginManager PluginManager { get; }
    }
}

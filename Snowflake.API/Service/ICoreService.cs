using System;
using System.Collections.Generic;
using Snowflake.Service.Manager;
using Snowflake.Service.HttpServer;
using Snowflake.Emulator.Configuration;
using Snowflake.Controller;
using Snowflake.Game;
using Snowflake.Platform;
namespace Snowflake.Service
{
    public interface ICoreService
    {
        IAjaxManager AjaxManager { get; }
        IBaseHttpServer APIServer { get; }
        IEmulatorAssembliesManager EmulatorManager { get; }
        string AppDataDirectory { get; }
        IConfigurationFlagDatabase ConfigurationFlagDatabase { get; }
        IControllerDatabase ControllerDatabase { get; }
        IControllerPortsDatabase ControllerPortsDatabase { get; }
        event EventHandler CoreLoaded;
        IGameDatabase GameDatabase { get; }
        IDictionary<string, IPlatformInfo> LoadedPlatforms { get; }
        IBaseHttpServer MediaStoreServer { get; }
        IPluginManager PluginManager { get; }
        IBaseHttpServer ThemeServer { get; }
    }
}

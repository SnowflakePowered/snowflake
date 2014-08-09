using System.Collections.Generic;
using System.Reflection;

namespace Snowflake.Plugin
{
    public interface IPlugin
    {
        string PluginName { get; }
        string PluginDataPath { get; }
        Assembly PluginAssembly { get; }
        IDictionary<string, dynamic> PluginInfo { get; }
    }
}

using System.Collections.Generic;
using System.Reflection;
using Snowflake.Service;
namespace Snowflake.Plugin
{
    public interface IBasePlugin
    {
        string PluginName { get; }
        string PluginDataPath { get; }
        Assembly PluginAssembly { get; }
        IDictionary<string, dynamic> PluginInfo { get; }
        FrontendCore CoreInstance { get; }
        //todo IFrontendCore (fix ater planned refactor)
    }
}

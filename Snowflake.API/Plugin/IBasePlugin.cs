using System.Collections.Generic;
using System.Reflection;
using Snowflake.Service;
using System.IO;
namespace Snowflake.Plugin
{
    public interface IBasePlugin
    {
        string PluginName { get; }
        string PluginDataPath { get; }
        Assembly PluginAssembly { get; }
        IDictionary<string, dynamic> PluginInfo { get; }
        ICoreService CoreInstance { get; }
        Stream GetResource(string resourceName);
        IPluginConfiguration PluginConfiguration { get; }
    }
}

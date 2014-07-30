using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.Composition;

namespace Snowflake.API.Interface.Plugin
{
    public interface IPlugin
    {
        string PluginName { get; }
        string PluginDataPath { get; }
        Assembly PluginAssembly { get; }
        IDictionary<string, dynamic> PluginInfo { get; }
    }
}

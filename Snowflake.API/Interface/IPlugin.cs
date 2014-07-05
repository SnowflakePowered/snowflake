using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Snowflake.API.Interface
{
    public interface IPlugin
    {
        string PluginName { get; }
        Assembly PluginAssembly { get; }
        Dictionary<string, dynamic> PluginInfo { get; }
    }
}

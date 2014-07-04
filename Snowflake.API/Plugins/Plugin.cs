using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.API.Interface;

namespace Snowflake.API.Plugins
{
    public class Plugin : IPlugin
    {
        public string PluginName { get; private set; }

        public Plugin(string pluginName)
        {
            this.PluginName = pluginName;
        }

    }
}

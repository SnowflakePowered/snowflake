using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Extensibility
{

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class PluginAttribute : Attribute
    {
    
        public PluginAttribute(string pluginName)
        {
            this.PluginName = pluginName;
        }

        public string PluginName { get; }

    }
}

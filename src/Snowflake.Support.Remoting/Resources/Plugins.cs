using Snowflake.Records.Game;
using Snowflake.Services;
using Snowflake.Support.Remoting.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Snowflake.Support.Remoting.Resources
{
    public class Plugins
    {

        private IPluginManager PluginManager{ get; }
        public Plugins(IPluginManager pluginManager)
        {
            this.PluginManager = pluginManager; 
        }

        
        [Endpoint(RequestVerb.Read, "~:plugins")]
        public IEnumerable<string> ListPlugins()
        {
            //todo: doesn't make sense without type.
            yield break;
        }

        [Endpoint(RequestVerb.Read, "~:plugins:{echo}")]
        public string Echo(string echo)
        {
            return echo;
        }

        [Endpoint(RequestVerb.Read, "~:plugins:{echo}:{concat}")]
        public string EchoConcat(string echo, string concat)
        {
            return echo;
        }
    }
}

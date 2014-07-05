using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.API.Interface;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using Snowflake.API.Constants.Plugin;
namespace Snowflake.API.Base
{
    public class Plugin : IPlugin
    {
        public string PluginName { get; private set; }
        public Dictionary<string, dynamic> PluginInfo { get; private set; }
        public Assembly PluginAssembly { get; private set; }
     
        

        public Plugin(Assembly pluginAssembly)
        {
            this.PluginAssembly = pluginAssembly;
            
            using (Stream stream = this.PluginAssembly.GetManifestResourceStream("plugin.json"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string file = reader.ReadToEnd();
                Console.WriteLine(file);
                Dictionary<string, dynamic> pluginInfo = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(file);
                this.PluginInfo = pluginInfo;
            }
            this.PluginName = PluginInfo[PluginInfoFields.Name];
        }

    }
}

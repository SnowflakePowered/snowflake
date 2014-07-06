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
using Snowflake.API.Configuration;
namespace Snowflake.API.Base
{
    public class BasePlugin : IPlugin
    {
        public string PluginName { get; private set; }
        public Dictionary<string, dynamic> PluginInfo { get; private set; }
        public Assembly PluginAssembly { get; private set; }
        public string PluginDataPath { get; private set; }
        public virtual IConfiguration PluginConfiguration { get; private set; }

        public BasePlugin(Assembly pluginAssembly)
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
            this.PluginDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake", "plugins", PluginName);
            if (!Directory.Exists(this.PluginDataPath)) Directory.CreateDirectory(this.PluginDataPath);
        }

        private virtual void InitConfiguration()
        {
            this.PluginConfiguration = new YamlConfiguration(Path.Combine(this.PluginDataPath, "config.yml"));
            this.PluginConfiguration.LoadConfiguration();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using Snowflake.Constants.Plugin;
using Snowflake.Service;
using Snowflake.Plugin;

namespace Snowflake.Plugin
{
    public abstract class BasePlugin : IBasePlugin
    {
        public string PluginName { get; private set; }
        public IDictionary<string, dynamic> PluginInfo { get; private set; }
        public Assembly PluginAssembly { get; private set; }
        public string PluginDataPath { get; private set; }
        public virtual IPluginConfiguration PluginConfiguration { get; private set; }
        public CoreService CoreInstance { get; private set; }
        protected BasePlugin(Assembly pluginAssembly, CoreService coreInstance)
        {
            this.PluginAssembly = pluginAssembly;
            this.CoreInstance = coreInstance;
            using (Stream stream = this.GetResource("plugin.json"))
            using (var reader = new StreamReader(stream))
            {
                string file = reader.ReadToEnd();
                var pluginInfo = JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(file);
                this.PluginInfo = pluginInfo;
            }
            this.PluginName = PluginInfo[PluginInfoFields.Name];
            this.PluginDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake", "plugins", PluginName);
            if (!Directory.Exists(this.PluginDataPath)) Directory.CreateDirectory(this.PluginDataPath);
        }

        protected virtual void InitConfiguration(string defaultValues)
        {
            this.PluginConfiguration = new YamlConfiguration(Path.Combine(this.PluginDataPath, "config.yml"), defaultValues);
            this.PluginConfiguration.LoadConfiguration();
        }

        protected virtual void InitConfiguration()
        {
            using (Stream stream = this.GetResource("config.yml"))
            using (var reader = new StreamReader(stream))
            {
                string file = reader.ReadToEnd();
                this.InitConfiguration(file);
            }
        }

        protected virtual Stream GetResource(string resourceName)
        {
            return this.GetResource(this.PluginAssembly.GetName().Name + ".resource." + resourceName);
        }
    }
}

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
using Snowflake.Plugin.Configuration;
using NLog;
namespace Snowflake.Plugin
{
    public abstract class BasePlugin : IBasePlugin
    {
        public string PluginName { get; private set; }
        public IDictionary<string, dynamic> PluginInfo { get; private set; }
        public Assembly PluginAssembly { get; private set; }
        public string PluginDataPath { get; private set; }
        public virtual IPluginConfiguration PluginConfiguration { get; protected set; }
        public virtual IList<IPluginConfigOption> PluginConfigurationOptions { get; protected set; }
        public ICoreService CoreInstance { get; private set; }
        public IList<string> SupportedPlatforms { get; private set; }
        protected ILogger Logger { get; private set; }
        protected BasePlugin(Assembly pluginAssembly, ICoreService coreInstance)
        {
            this.PluginAssembly = pluginAssembly;
            this.CoreInstance = coreInstance;
            string file = this.GetStringResource("plugin.json");
            var pluginInfo = JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(file);
            this.PluginInfo = pluginInfo;
            
            this.PluginName = this.PluginInfo[PluginInfoFields.Name];
            this.Logger = LogManager.GetLogger(this.PluginName);
            this.SupportedPlatforms = this.PluginInfo[PluginInfoFields.SupportedPlatforms].ToObject<IList<string>>();
            this.PluginDataPath = Path.Combine(coreInstance.AppDataDirectory, "plugins", PluginName);
            if (!Directory.Exists(this.PluginDataPath)) Directory.CreateDirectory(this.PluginDataPath);
        }
        public void LoadConfigurationOptions()
        {
            this.PluginConfigurationOptions = new List<IPluginConfigOption>();
        }
        public Stream GetResource(string resourceName)
        {
            return this.PluginAssembly.GetManifestResourceStream($"{this.PluginAssembly.GetName().Name}.resource.{resourceName}");
        }
        public string GetStringResource(string resourceName)
        {
            using (Stream stream = this.GetResource(resourceName))
            using (var reader = new StreamReader(stream))
            {
                string file = reader.ReadToEnd();
                return file;
            }
        }

        public virtual void Dispose()
        {
            
           this.PluginConfiguration?.SaveConfiguration();
        }
    }
}

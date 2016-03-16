using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using NLog;
using Snowflake.Constants.Plugin;
using Snowflake.Extensibility.Configuration;
using Snowflake.Service;

namespace Snowflake.Extensibility
{
    public abstract class Plugin : IPlugin
    {
        public string PluginName { get; }
        public IDictionary<string, dynamic> PluginInfo { get; }
        public Assembly PluginAssembly { get; }
        public string PluginDataPath { get; }
        public virtual IPluginConfiguration PluginConfiguration { get; protected set; }
        public virtual IList<IPluginConfigOption> PluginConfigurationOptions { get; protected set; }
        public ICoreService CoreInstance { get; }
        public IList<string> SupportedPlatforms { get; }
        protected ILogger Logger { get; private set; }
        protected Plugin(ICoreService coreInstance)
        {
            this.PluginName = this.GetPluginName();
            this.PluginAssembly = this.GetType().Assembly;
            this.CoreInstance = coreInstance;
            string file = this.GetStringResource("plugin.json");
            var pluginInfo = JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(file, new JsonSerializerSettings() {Culture = CultureInfo.InvariantCulture});
            this.PluginInfo = pluginInfo;
            this.Logger = LogManager.GetLogger(this.PluginName);
            this.SupportedPlatforms = this.PluginInfo[PluginInfoFields.SupportedPlatforms].ToObject<IList<string>>();
            this.PluginDataPath = Path.Combine(coreInstance.AppDataDirectory, "plugins", this.PluginName);
            if (!Directory.Exists(this.PluginDataPath)) Directory.CreateDirectory(this.PluginDataPath);
        }
        public void LoadConfigurationOptions()
        {
            this.PluginConfigurationOptions = new List<IPluginConfigOption>();
        }
        public Stream GetResource(string resourceName)
        {
            var pluginName = this.PluginName.Replace('-', '_'); //the compiler replaces all dashes in resource names with underscores
            resourceName = resourceName.Replace('-', '_');
            return this.PluginAssembly.GetManifestResourceStream($"{this.PluginAssembly.GetName().Name}.resource.{pluginName}.{resourceName}");
        }

        public string GetPluginName()
        {
            return this.GetType().GetCustomAttribute<PluginAttribute>().PluginName;
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

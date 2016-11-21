using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using Snowflake.Extensibility.Configuration;
using Snowflake.Services;

namespace Snowflake.Extensibility
{
    public abstract class Plugin : IPlugin
    {
        public string PluginName { get; }
        public IPluginProperties PluginProperties { get; }
        public string PluginDataPath { get; }
        public virtual IPluginConfiguration PluginConfiguration { get; protected set; }
        public virtual IList<IPluginConfigOption> PluginConfigurationOptions { get; protected set; }

        /// <summary>
        /// The logger provided for this plugin
        /// </summary>
        protected ILogger Logger { get; private set; }
        /// <summary>
        /// The Assembly object representation of the compiled plugin
        /// </summary>
        protected Assembly PluginAssembly { get; }

        protected Plugin(string appDataDirectory, IPluginProperties pluginProperties)
        {
            this.PluginName = this.GetPluginName();
            this.PluginAssembly = this.GetType().GetTypeInfo().Assembly;
            this.Logger = LogManager.GetLogger(this.PluginName);
            this.PluginProperties = pluginProperties;
            this.PluginDataPath = Path.Combine(appDataDirectory, "plugins", this.PluginName);

            if (!Directory.Exists(this.PluginDataPath))
                Directory.CreateDirectory(this.PluginDataPath);
        }

        protected Plugin(string appDataDirectory)
        {
            this.PluginName = this.GetPluginName();
            this.PluginAssembly = this.GetType().GetTypeInfo().Assembly;
            this.Logger = LogManager.GetLogger(this.PluginName);
            this.PluginProperties = new JsonPluginProperties(JObject
               .FromObject(JsonConvert
               .DeserializeObject(this.GetStringResource("plugin.json"),
                                   new JsonSerializerSettings { Culture = CultureInfo.InvariantCulture })));
            this.PluginDataPath = Path.Combine(appDataDirectory, "plugins", this.PluginName);

            if (!Directory.Exists(this.PluginDataPath))
                Directory.CreateDirectory(this.PluginDataPath);
        }

        public void LoadConfigurationOptions()
        {
            this.PluginConfigurationOptions = new List<IPluginConfigOption>();
        }

        /// <summary>
        /// Gets an embedded resource as a Stream from the plugin namespace.
        /// Wraps GetManifestResourceStream so that specifiying the full namespace of the resource is not required
        /// </summary>
        /// <param name="resourceName">The name of the resource</param>
        /// <returns>The resource as a stream</returns>
        protected Stream GetResource(string resourceName) => this.GetSiblingResource(this.PluginName, resourceName);


        /// <summary>
        /// Gets an embedded resource as a Stream from the same assembly belonging to a sibling namespace
        /// </summary>
        /// <param name="resourceName">The name of the resource</param>
        /// <param name="siblingPluginName">The name of the sibling resource</param>
        /// <returns>The resource as a stream</returns>
        protected Stream GetSiblingResource(string siblingPluginName, string resourceName)
        {
            var pluginName = siblingPluginName.Replace('-', '_'); //the compiler replaces all dashes in resource names with underscores
            resourceName = resourceName.Replace('-', '_');
            return this.PluginAssembly.GetManifestResourceStream($"{this.PluginAssembly.GetName().Name}.resource.{pluginName}.{resourceName}");
        }

        /// <summary>
        /// Gets all resource names for a sibling plugin
        /// </summary>
        /// <param name="siblingPluginName"></param>
        /// <returns></returns>
        protected IEnumerable<string> GetAllSiblingResourceNames(string siblingPluginName)
        {
            var pluginName = siblingPluginName.Replace('-', '_'); //the compiler replaces all dashes in resource names with underscores
            return from names in this.PluginAssembly.GetManifestResourceNames()
                where names.StartsWith($"{this.PluginAssembly.GetName().Name}.resource.{pluginName}.")
                select names.Replace($"{this.PluginAssembly.GetName().Name}.resource.{pluginName}.", "");
        }

        /// <summary>
        /// Gets all resource names
        /// </summary>
        /// <returns>The names of the resources</returns>
        protected IEnumerable<string> GetAllResourceNames() => this.GetAllSiblingResourceNames(this.PluginName);

        /// <summary>
        /// Gets an embedded resource as a String from the plugin Assembly
        /// </summary>
        /// <param name="resourceName">The name of the resource</param>
        /// <returns>The resource as a string</returns>
        protected string GetStringResource(string resourceName)
            => this.GetSiblingStringResource(this.PluginName, resourceName);

        /// <summary>
        /// Gets an embedded resource as a String from the same assembly belonging to a sibling namespace
        /// </summary>
        /// <param name="resourceName">The name of the resource</param>
        /// <param name="siblingPluginName">The name of the sibling resource</param>
        /// <returns>The resource as a stream</returns>
        protected string GetSiblingStringResource(string siblingPluginName, string resourceName)
        {
            using (Stream stream = this.GetSiblingResource(siblingPluginName, resourceName))
            using (var reader = new StreamReader(stream))
            {
                string file = reader.ReadToEnd();
                return file;
            }
        }

        private string GetPluginName()
        {
            return this.GetType().GetTypeInfo().GetCustomAttribute<PluginAttribute>().PluginName;
        }

        public virtual void Dispose()
        {
            
           this.PluginConfiguration?.SaveConfiguration();
        }
    }
}

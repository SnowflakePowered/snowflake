using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Snowflake.Extensibility;
using Snowflake.Loader;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Reflection;
using Snowflake.Utility;
using System.IO;
using Snowflake.Extensions;
using System.Collections.Immutable;
using Snowflake.Extensibility.Provisioned;

namespace Snowflake.Support.PluginManager
{
    public class PluginManager : IPluginManager
    {
        private readonly ILogProvider logProvider;
        private readonly IContentDirectoryProvider contentDirectory;
        private readonly IDictionary<Type, IImmutableList<IPlugin>> loadedPlugins;

        public PluginManager(ILogProvider logProvider, IContentDirectoryProvider contentDirectory)
        {
            this.logProvider = logProvider;
            this.contentDirectory = contentDirectory;
            this.loadedPlugins = new Dictionary<Type, IImmutableList<IPlugin>>();
        }

        public IPluginProvision GetProvision<T>(IModule composableModule) where T : IPlugin
        {
            var resourceDirectory = composableModule.ContentsDirectory
                                    .CreateSubdirectory("resource"); //todo: check for missing directory!!
            var pluginAttr = typeof(T).GetTypeInfo().GetCustomAttribute<PluginAttribute>();
            if (pluginAttr.PluginName == "common") throw new UnauthorizedAccessException("Plugin name can not be 'common'.");
            var pluginResourceDirectory = resourceDirectory.CreateSubdirectory(pluginAttr.PluginName);
            var pluginCommonResourceDirectory = resourceDirectory.CreateSubdirectory("common");
            IPluginProperties properties = new JsonPluginProperties(JObject
               .FromObject(JsonConvert
               .DeserializeObject(File.ReadAllText(pluginResourceDirectory.GetFiles().Where(f => f.Name == "plugin.json")
               .First().FullName)), new JsonSerializer { Culture = CultureInfo.InvariantCulture }));
            var pluginDataDirectory = this.contentDirectory.ApplicationData.CreateSubdirectory("plugincontents")
                    .CreateSubdirectory(pluginAttr.PluginName);
            return new PluginProvision(this.logProvider.GetLogger($"Plugin:{pluginAttr.PluginName}"),
                properties, pluginAttr.PluginName, 
                properties.Get(PluginInfoFields.Author) ?? pluginAttr.Author,
                properties.Get(PluginInfoFields.Description) ?? pluginAttr.Description,
                pluginAttr.Version, pluginDataDirectory, pluginCommonResourceDirectory, pluginResourceDirectory);
        }

        public IEnumerable<T> Get<T>() where T : IPlugin
        {
            if(this.loadedPlugins.ContainsKey(typeof(T)))
            {
                return this.loadedPlugins[typeof(T)].Cast<T>().ToImmutableList();
            }
            return ImmutableList<T>.Empty;
        }

        public T Get<T>(string pluginName) where T : IPlugin
        {
            return (T)this.loadedPlugins[typeof(T)].FirstOrDefault(p => p.Name == pluginName);
        }

        public IPlugin Get(string pluginName)
        {
            return this.loadedPlugins.SelectMany(p => p.Value).FirstOrDefault(p => p.Name == pluginName);
        }

        public void Register<T>(T plugin) where T : IPlugin
        {
            if (!this.loadedPlugins.ContainsKey(typeof(T)))
                this.loadedPlugins.Add(typeof(T), ImmutableList<IPlugin>.Empty);
            if (this.IsRegistered(plugin.Name))
            {
                throw new InvalidOperationException($"Plugin {plugin.Name} is already registered.");
            }
            this.loadedPlugins[typeof(T)] = this.loadedPlugins[typeof(T)].Add(plugin);
        }

        public bool IsRegistered<T>(string pluginName) where T : IPlugin
        {
            return this.Get<T>(pluginName) != null;
        }

        public bool IsRegistered(string pluginName)
        {
            return this.loadedPlugins.SelectMany(p => p.Value).Any(p => p.Name == pluginName);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~PluginManager() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


    }
}

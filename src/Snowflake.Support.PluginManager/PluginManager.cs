using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Snowflake.Extensibility;
using Snowflake.Extensibility.Configuration;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Loader;
using Snowflake.Services;

namespace Snowflake.Support.PluginManager
{
    public class PluginManager : IPluginManager
    {
        private readonly ILogProvider logProvider;
        private readonly IContentDirectoryProvider contentDirectory;
        private readonly IDictionary<Type, IImmutableList<IPlugin>> loadedPlugins;
        private readonly ISqliteDatabaseProvider databaseProvider;

        public PluginManager(ILogProvider logProvider,
            IContentDirectoryProvider contentDirectory,
            ISqliteDatabaseProvider databaseProvider)
        {
            this.logProvider = logProvider;
            this.contentDirectory = contentDirectory;
            this.loadedPlugins = new Dictionary<Type, IImmutableList<IPlugin>>();
            this.databaseProvider = databaseProvider;
        }

        /// <inheritdoc/>
        public IPluginProvision GetProvision<T>(IModule composableModule)
            where T : IPlugin
        {
            var resourceDirectory = composableModule.ContentsDirectory
                                    .CreateSubdirectory("resource"); // todo: check for missing directory!!
            var pluginAttr = typeof(T).GetTypeInfo().GetCustomAttribute<PluginAttribute>();
            if (pluginAttr == null)
            {
                throw new InvalidOperationException($"Can not load provision for {typeof(T)} without a PluginAttribute");
            }

            if (pluginAttr.PluginName == "common")
            {
                throw new UnauthorizedAccessException("Plugin name can not be 'common'.");
            }

            var pluginResourceDirectory = resourceDirectory.CreateSubdirectory(pluginAttr.PluginName);
            var pluginCommonResourceDirectory = resourceDirectory.CreateSubdirectory("common");
            var pluginJsonFile = pluginResourceDirectory.GetFiles()
                .FirstOrDefault(f => f.Name == "plugin.json");
            if (pluginJsonFile == null)
            {
                throw new FileNotFoundException($"Unable to find plugin.json for {pluginAttr.PluginName}");
            }

            IPluginProperties properties = new JsonPluginProperties(JObject
               .FromObject(JsonConvert
               .DeserializeObject(File.ReadAllText(pluginJsonFile.FullName)), new JsonSerializer { Culture = CultureInfo.InvariantCulture }));
            var pluginDataDirectory = this.contentDirectory.ApplicationData.CreateSubdirectory("plugincontents")
                    .CreateSubdirectory(pluginAttr.PluginName);
            var pluginConfigDb = this.databaseProvider.CreateDatabase("pluginconfiguration", $"{pluginAttr.PluginName}.Configuration");
            var pluginStore = new SqlitePluginConfigurationStore(pluginConfigDb);
            return new PluginProvision(this.logProvider.GetLogger($"Plugin:{pluginAttr.PluginName}"),
                properties,
                pluginStore,
                pluginAttr.PluginName,
                properties.Get(PluginInfoFields.Author) ?? pluginAttr.Author,
                properties.Get(PluginInfoFields.Description) ?? pluginAttr.Description,
                pluginAttr.Version, pluginDataDirectory, pluginCommonResourceDirectory, pluginResourceDirectory);
        }

        /// <inheritdoc/>
        public IEnumerable<T> Get<T>()
            where T : IPlugin
        {
            if (this.loadedPlugins.ContainsKey(typeof(T)))
            {
                return this.loadedPlugins[typeof(T)].Cast<T>().ToImmutableList();
            }

            return ImmutableList<T>.Empty;
        }

        /// <inheritdoc/>
        public T Get<T>(string pluginName)
            where T : IPlugin
        {
            return (T)this.loadedPlugins[typeof(T)].FirstOrDefault(p => p.Name == pluginName);
        }

        /// <inheritdoc/>
        public IProvisionedPlugin Get(string pluginName)
        {
            return this.loadedPlugins.SelectMany(p => p.Value)
                .Where(p => p is IProvisionedPlugin)
                .Cast<IProvisionedPlugin>()
                .FirstOrDefault(p => p.Name == pluginName);
        }

        /// <inheritdoc/>
        public void Register<T>(T plugin)
            where T : IPlugin
        {
            if (!this.loadedPlugins.ContainsKey(typeof(T)))
            {
                this.loadedPlugins.Add(typeof(T), ImmutableList<IPlugin>.Empty);
            }

            if (this.IsRegistered(plugin.Name))
            {
                throw new InvalidOperationException($"Plugin {plugin.Name} is already registered.");
            }

            this.loadedPlugins[typeof(T)] = this.loadedPlugins[typeof(T)].Add(plugin);
        }

        /// <inheritdoc/>
        public bool IsRegistered<T>(string pluginName)
            where T : IPlugin
        {
            return this.Get<T>(pluginName) != null;
        }

        /// <inheritdoc/>
        public bool IsRegistered(string pluginName)
        {
            return this.loadedPlugins.SelectMany(p => p.Value).Any(p => p.Name == pluginName);
        }

        /// <inheritdoc/>
        public IPluginCollection<T> GetCollection<T>()
            where T : IPlugin
        {
            return new PluginCollection<T>(this);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (IPlugin plugin in this.loadedPlugins.Values)
                    {
                        plugin.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.

        /// <inheritdoc/>
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Snowflake.Extensibility;
using Snowflake.Extensibility.Configuration;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Filesystem;
using Snowflake.Loader;
using Snowflake.Services;
using Zio;
using FS = Snowflake.Filesystem;
namespace Snowflake.Support.PluginManager
{
    /// <inheritdoc />
    public class PluginManager : IPluginManager
    {
        private readonly ILogProvider logProvider;
        private readonly IContentDirectoryProvider contentDirectory;
        private readonly IDictionary<Type, IImmutableList<IPlugin>> loadedPlugins;
        private readonly IPluginConfigurationStore configurationStore;
        private readonly IFileSystem rootFs;

        public ILogger Logger { get; }

        /// <summary>
        /// Initializes the default plugin manager.
        /// </summary>
        /// <param name="logProvider">The logging provider.</param>
        /// <param name="contentDirectory">The content directory provider.</param>
        /// <param name="databaseProvider">The plugin configuration store.</param>
        /// <param name="rootFs">The root file system used to create new subfilesystems.</param>
        public PluginManager(ILogProvider logProvider,
            IContentDirectoryProvider contentDirectory,
            IPluginConfigurationStore databaseProvider,
            IFileSystem rootFs)
        {
            this.logProvider = logProvider;
            this.contentDirectory = contentDirectory;
            this.loadedPlugins = new Dictionary<Type, IImmutableList<IPlugin>>();
            this.configurationStore = databaseProvider;
            this.rootFs = rootFs;
            this.Logger = this.logProvider.GetLogger("PluginManager");
        }

        /// <inheritdoc/>
        public IPluginProvision GetProvision<T>(IModule composableModule)
            where T : class, IPlugin
        {
            var appDataPath = rootFs.ConvertPathFromInternal(this.contentDirectory.ApplicationData.FullName);

            var resourcePath = rootFs.ConvertPathFromInternal(composableModule.ContentsDirectory.FullName) / "resource";

            var pluginAttr = typeof(T).GetTypeInfo().GetCustomAttribute<PluginAttribute>();
            if (pluginAttr == null)
            {
                throw new InvalidOperationException(
                    $"Can not load provision for {typeof(T)} without a PluginAttribute");
            }

            if (pluginAttr.PluginName == "common")
            {
                throw new UnauthorizedAccessException("Plugin name can not be 'common'.");
            }

            var pluginResourceDirectory = resourcePath / pluginAttr.PluginName;
            var pluginCommonResourceDirectory = resourcePath / "common";

            var pluginResourceFs = rootFs.GetOrCreateSubFileSystem(pluginResourceDirectory);
            var pluginResourceDir = new FS.Directory(pluginResourceFs);

            var pluginCommonFs = rootFs.GetOrCreateSubFileSystem(pluginCommonResourceDirectory);
            var pluginCommonDir = new FS.Directory(pluginCommonFs);

            var pluginJsonFile = pluginResourceDir.EnumerateFilesRecursive()
                .FirstOrDefault(f => f.Name == "plugin.json");
            if (pluginJsonFile == null)
            {
                throw new FileNotFoundException($"Unable to find plugin.json for {pluginAttr.PluginName}");
            }

            IPluginProperties properties = new JsonPluginProperties(JObject
                .FromObject(JsonConvert
                        .DeserializeObject(pluginJsonFile.ReadAllText()),
                    new JsonSerializer {Culture = CultureInfo.InvariantCulture}));

            var pluginDataFs = rootFs.GetOrCreateSubFileSystem(appDataPath / "plugindata" / pluginAttr.PluginName);

            return new PluginProvision(this.logProvider.GetLogger($"Plugin:{pluginAttr.PluginName}"),
                properties,
                this.configurationStore,
                pluginAttr.PluginName,
                properties.Get(PluginInfoFields.Author) ?? pluginAttr.Author,
                properties.Get(PluginInfoFields.Description) ?? pluginAttr.Description,
                pluginAttr.Version, composableModule.ContentsDirectory, 
                new FS.Directory(pluginDataFs),
                pluginCommonDir.AsReadOnly(), pluginResourceDir.AsReadOnly());
        }

        /// <inheritdoc/>
        public IEnumerable<T> Get<T>()
            where T : class, IPlugin
        {
            if (this.loadedPlugins.ContainsKey(typeof(T)))
            {
                return this.loadedPlugins[typeof(T)].Cast<T>().ToImmutableList();
            }

            return ImmutableList<T>.Empty;
        }

        /// <inheritdoc/>
        public T? Get<T>(string pluginName)
            where T : class, IPlugin
        {
            if (!this.loadedPlugins.TryGetValue(typeof(T), out var pluginCollection)) return null;
            return (T?) pluginCollection.FirstOrDefault(p => p.Name == pluginName);
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
            where T : class, IPlugin
        {
            if (!this.loadedPlugins.ContainsKey(typeof(T)))
            {
                this.loadedPlugins.Add(typeof(T), ImmutableList<IPlugin>.Empty);
                this.Logger.Info($"Found new plugin type {typeof(T).Name}.");
            }

            if (this.IsRegistered(plugin.Name))
            {
                this.Logger.Error($"Plugin {plugin.Name} is already registered.");
                throw new InvalidOperationException($"Plugin {plugin.Name} is already registered.");
            }

            this.loadedPlugins[typeof(T)] = this.loadedPlugins[typeof(T)].Add(plugin);
            this.Logger.Info($"Registered {typeof(T).Name} plugin {plugin.Name}.");
        }

        /// <inheritdoc/>
        public bool IsRegistered<T>(string pluginName)
            where T : class, IPlugin
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
            where T : class, IPlugin
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
                    foreach (IPlugin plugin in this.loadedPlugins.Values.SelectMany(i => i))
                    {
                        plugin.Dispose();
                        this.Logger.Info($"Disposed plugin {plugin.Name}.");
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

        #region IEnumerable Support

        /// <inheritdoc />
        public IEnumerator<IPlugin> GetEnumerator()
        {
            return this.loadedPlugins.SelectMany(p => p.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}

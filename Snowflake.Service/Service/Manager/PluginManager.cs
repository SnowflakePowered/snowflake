using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Snowflake.Emulator;
using Snowflake.Extensions;
using Snowflake.Plugin;
using Snowflake.Plugin.Configuration;
using Snowflake.Scraper;

namespace Snowflake.Service.Manager
{

    public class PluginManager : IPluginManager
    {
        public string LoadablesLocation { get; }
        public bool IsInitialized { get; private set; }
        private readonly IDictionary<string, Type> registry;
        public IReadOnlyDictionary<string, Type> Registry => this.registry.AsReadOnly();
        public ICoreService CoreInstance { get; }
        private readonly IList<Type> importingTypes;
        private readonly IDictionary<Type, IDictionary<string, IBasePlugin>> loadedPlugins;

        public PluginManager(string loadablesLocation, ICoreService coreInstance, params Type[] pluginTypes)
        {
            this.CoreInstance = coreInstance;
            this.LoadablesLocation = loadablesLocation;
            this.loadedPlugins = new Dictionary<Type, IDictionary<string, IBasePlugin>>();
            this.registry = new Dictionary<string, Type>();
            this.importingTypes = new List<Type>();
            var addType = typeof (PluginManager).GetMethod("AddType");
            foreach (Type T in pluginTypes)
            {
                addType.MakeGenericMethod(T)?.Invoke(this, null);
            }
        }

        public void AddType<T>() where T: IBasePlugin
        {
            this.importingTypes.Add(typeof(T));
            this.loadedPlugins.Add(typeof(T), new Dictionary<string, IBasePlugin>());
        }
        public void Initialize()
        {
            if (this.IsInitialized) return;
            foreach (Type T in this.importingTypes)
            {
                //We have to use reflection as we only know what type T is at runtime
                var composeImports = typeof(PluginManager).GetMethod("ComposeImports", BindingFlags.NonPublic | BindingFlags.Instance); 
                composeImports.MakeGenericMethod(T).Invoke(this, null);
            }
            this.IsInitialized = true;
        }

        public IDictionary<string, T> Plugins<T>() where T : IBasePlugin
        {
            return this.loadedPlugins[typeof (T)].ToDictionary(plugin => plugin.Key, plugin => (T)plugin.Value);
        }

        public T Plugin<T>(string pluginName) where T : IBasePlugin
        {
            return (T) this.loadedPlugins[typeof (T)][pluginName];
        }

        private void ComposeImports<T>() where T : IBasePlugin
        {
            if (!Directory.Exists(Path.Combine(this.LoadablesLocation, "plugins"))) Directory.CreateDirectory(Path.Combine(this.LoadablesLocation, "plugins"));

            var catalog = new DirectoryCatalog(Path.Combine(this.LoadablesLocation, "plugins"));
            var container = new CompositionContainer(catalog);
            container.ComposeExportedValue("coreInstance", this.CoreInstance);
            container.ComposeParts();
            var exports = container.GetExports<T>(); //Only initialize exports of type T
            foreach (var plugin in exports)
            {
                try
                {
                    this.loadedPlugins[typeof (T)].Add(plugin.Value.PluginName, plugin.Value); //initialize and load plugins of type T
                    this.registry.Add(plugin.Value.PluginName, typeof (T));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to load plugin: {ex.Source}");
                    Console.WriteLine(ex.ToString());
                }
            }

        }

        bool disposed;
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            if (disposing)
            {
                foreach (var plugin in this.Registry)
                {
                    this.loadedPlugins[plugin.Value][plugin.Key].Dispose();
                    this.loadedPlugins[plugin.Value].Remove(plugin.Key);
                }
            }
            this.disposed = true;
        }
    }
}

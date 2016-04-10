using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.ReflectionModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using NLog;
using Snowflake.Constants.Plugin;
using Snowflake.Emulator;
using Snowflake.Events;
using Snowflake.Extensions;
using Snowflake.Extensibility;
using Snowflake.Extensibility.Configuration;
using Snowflake.Scraper;
using Snowflake.Events.ServiceEvents;

namespace Snowflake.Service.Manager
{

    public class PluginManager : IPluginManager
    {
        public string LoadablesLocation { get; }
        public bool IsInitialized { get; private set; }
        private readonly IDictionary<string, Type> registry;
        public IReadOnlyDictionary<string, Type> Registry => this.registry.AsReadOnly();
        public ICoreService CoreInstance { get; }
        private readonly IDictionary<Type, IDictionary<string, IPlugin>> loadedPlugins;

        public PluginManager(string loadablesLocation, ICoreService coreInstance)
        {
            this.CoreInstance = coreInstance;
            this.LoadablesLocation = loadablesLocation;
            this.loadedPlugins = new Dictionary<Type, IDictionary<string, IPlugin>>();
            this.registry = new Dictionary<string, Type>();
        }

    
        public void Initialize()
        {
            if (this.IsInitialized) return;
            this.ComposeImports();
            this.IsInitialized = true;
        }

        public IDictionary<string, T> Get<T>() where T : IPlugin
        {
            try
            {
                return this.loadedPlugins[typeof (T)].ToDictionary(plugin => plugin.Key, plugin => (T) plugin.Value);
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public T Get<T>(string pluginName) where T : IPlugin
        {
            return (T) this.loadedPlugins[typeof (T)][pluginName];
        }

        public void Register<T>(T plugin) where T : IPlugin
        {
            if (!this.loadedPlugins.ContainsKey(typeof (T)))
                this.loadedPlugins.Add(typeof (T), new Dictionary<string, IPlugin>());
            this.loadedPlugins[typeof (T)].Add(plugin.PluginName, plugin);
            this.registry.Add(plugin.PluginName, typeof(T));
            SnowflakeEventManager.EventSource?.RaiseEvent(new PluginLoadedEventArgs(this.CoreInstance, plugin));
        }

     
        private void ComposeImports() 
        {
            if (!Directory.Exists(Path.Combine(this.LoadablesLocation, "plugins"))) Directory.CreateDirectory(Path.Combine(this.LoadablesLocation, "plugins"));
            var catalog = new DirectoryCatalog(Path.Combine(this.LoadablesLocation, "plugins"));
            var container = new CompositionContainer(catalog);
            container.ComposeParts();
            var exports = container.GetExports<IPluginContainer>(); //Only initialize exports of type T
           

            foreach (var pluginContainer 
                in from pluginContainers in exports
                   let pluginContainer = pluginContainers.Value
                   let loadPriority = pluginContainer
                        .GetType()
                        .GetCustomAttribute<ContainerLoadPriorityAttribute>(true)?
                        .LoadPriority ?? ContainerLoadPriority.Default
                   orderby loadPriority ascending
                   select pluginContainer)
            {
                try
                {
                    pluginContainer.Compose(this.CoreInstance);
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to load plugin container: {pluginContainer?.GetType().AssemblyQualifiedName}");
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

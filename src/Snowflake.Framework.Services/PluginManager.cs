using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using NLog;
using Snowflake.Extensions;
using Snowflake.Extensibility;
using Snowflake.Extensibility.Configuration;
using Snowflake.Scraper;
using System.Runtime.Loader;
using Snowflake.Utility;
using Snowflake.Loader;

namespace Snowflake.Services
{

    internal class PluginManager : IPluginManager
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
         //   SnowflakeEventManager.EventSource?.RaiseEvent(new PluginLoadedEventArgs(this.CoreInstance, plugin));
        }

     
        private IEnumerable<IComposer> GetExports(string loadableLocation)
        {
            
            foreach (string fileName in Directory.EnumerateFiles(loadableLocation)
                .Where(f => Path.GetExtension(f) == ".dll"))
            {
                Assembly assembly;
                IEnumerable<IComposer> containerTypes;
                try
                {
                    assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(fileName); //todo check for dupes
                    containerTypes = assembly.ExportedTypes
                    .Where(t => t.GetInterfaces().Contains(typeof(IComposer)))
                    .Where(t => t.GetConstructor(Type.EmptyTypes) != null)
                    .Select(t => Instantiate.CreateInstance(t) as IComposer);
                }
                catch
                {
                    Console.WriteLine("Unable to load assembly " + fileName);
                    continue;
                }

                    foreach (var container in containerTypes)
                    {
                        yield return container; //todo make into single linq
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

﻿using System;
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
using System.Runtime.Loader;
using Snowflake.Utility;

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
            this.ComposeImports();
            this.IsInitialized = true;
        }

        public IDictionary<string, T> Get<T>() where T : IPlugin
        {
            try
            {
                return this.loadedPlugins[typeof(T)].ToDictionary(plugin => plugin.Key, plugin => (T)plugin.Value);
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public T Get<T>(string pluginName) where T : IPlugin
        {
            return (T)this.loadedPlugins[typeof(T)][pluginName];
        }

        public void Register<T>(T plugin) where T : IPlugin
        {
            if (!this.loadedPlugins.ContainsKey(typeof(T)))
                this.loadedPlugins.Add(typeof(T), new Dictionary<string, IPlugin>());
            this.loadedPlugins[typeof(T)].Add(plugin.PluginName, plugin);
            this.registry.Add(plugin.PluginName, typeof(T));
        }

        private IEnumerable<Assembly> GetAssemblies()
        {
            string loadablesLocation = Path.Combine(this.LoadablesLocation, "plugins");
            foreach(string file in Directory.GetFiles(loadablesLocation, "*.dll", SearchOption.TopDirectoryOnly))
            {
                Assembly toYield = null;
                try
                {
                    toYield = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                    
                }
                catch (FileLoadException)
                {
                    toYield = Assembly.Load(AssemblyLoadContext.GetAssemblyName(file));
                }
                if (toYield != null) yield return toYield;
            }
        }


        private void ComposeImports()
        {
            string loadablesLocation = Path.Combine(this.LoadablesLocation, "plugins");

            if (!Directory.Exists(loadablesLocation)) Directory.CreateDirectory(loadablesLocation);
            var assemblies = this.GetAssemblies().ToList();
            var exports =  assemblies.SelectMany(asm => asm.ExportedTypes)
                .Where(t => t.GetInterfaces().Contains(typeof(IPluginContainer)))
                    .Where(t => t.GetConstructor(Type.EmptyTypes) != null)
                    .Select(t => Instantiate.CreateInstance(t) as IPluginContainer);

            foreach (var pluginContainer
                in from pluginContainers in exports
                   let loadPriority = pluginContainers
                        .GetType()
                        .GetTypeInfo()
                        .GetCustomAttribute<ContainerLoadPriorityAttribute>(true)?
                        .LoadPriority ?? ContainerLoadPriority.Default
                   orderby loadPriority ascending
                   select pluginContainers)
            {
                try
                {
                    pluginContainer.Compose(this.CoreInstance);
                    Console.WriteLine($"Loaded container : {pluginContainer?.GetType().Name}");
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
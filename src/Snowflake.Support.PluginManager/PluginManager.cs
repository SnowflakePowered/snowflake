using Snowflake.Extensibility;
using System;
using System.Collections.Generic;
using Snowflake.Utility;
using System.Linq;

namespace Snowflake.Support.PluginManager
{
    public class PluginManager
    {
        private readonly IDictionary<Type, IDictionary<string, IPlugin>> loadedPlugins;

        private readonly IDictionary<string, Type> registry;
        public IReadOnlyDictionary<string, Type> Registry => this.registry.AsReadOnly();

        public PluginManager()
        {
            this.loadedPlugins = new Dictionary<Type, IDictionary<string, IPlugin>>();
            this.registry = new Dictionary<string, Type>();
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
            //   SnowflakeEventManager.EventSource?.RaiseEvent(new PluginLoadedEventArgs(this.CoreInstance, plugin));
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Snowflake.Services;

namespace Snowflake.Extensibility
{
    public class PluginCollection<T> : IPluginCollection<T>
        where T: IPlugin
    {
        private IPluginManager PluginManager { get; }
        public PluginCollection(IPluginManager manager)
        {
            this.PluginManager = manager;
        }

        public T this[string pluginName] => this.PluginManager.Get<T>(pluginName);

        public IEnumerator<T> GetEnumerator() => this.PluginManager.Get<T>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.PluginManager.Get<T>().GetEnumerator();
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Extensibility.Configuration;

namespace Snowflake.Extensibility.Provisioning.Standalone
{
    internal class EmptyPluginConfigurationStore : IPluginConfigurationStore
    {
        private static readonly EmptyPluginConfigurationStore emptyPluginConfigurationStore =
            new EmptyPluginConfigurationStore();

        public static EmptyPluginConfigurationStore EmptyConfigurationStore
        {
            get => emptyPluginConfigurationStore;
        }

        public void Set(IConfigurationValue value)
        {
            return;
        }

        public IConfigurationSection<T> Get<T>()
            where T : class, IConfigurationSection<T>
        {
            return new ConfigurationSection<T>();
        }

        public void Set<T>(IConfigurationSection<T> configuration)
            where T : class, IConfigurationSection<T>
        {
            return;
        }

        public void Set(IEnumerable<IConfigurationValue> values)
        {
            return;
        }

        Task<IConfigurationSection<T>> IPluginConfigurationStore.GetAsync<T>()
        {
            return Task.FromResult((IConfigurationSection<T>)new ConfigurationSection<T>());
        }

        Task IPluginConfigurationStore.SetAsync<T>(IConfigurationSection<T> configuration)
        {
            return Task.CompletedTask;
        }

        public Task SetAsync(IConfigurationValue value)
        {
            return Task.CompletedTask;
        }

        public Task SetAsync(IEnumerable<IConfigurationValue> values)
        {
            return Task.CompletedTask;
        }
    }
}

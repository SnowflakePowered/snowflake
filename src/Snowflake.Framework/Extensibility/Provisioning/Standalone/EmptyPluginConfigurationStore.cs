using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Generators;
using Snowflake.Configuration.Internal;
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

        public void Set(Guid valueGuid, object? value)
        {
            return;
        }

        [GenericTypeAcceptsConfigurationSection(0)]
        public IConfigurationSection<T> Get<T>()
            where T : class
        {
            return new ConfigurationSection<T>();
        }

        [GenericTypeAcceptsConfigurationSection(0)]
        public void Set<T>(IConfigurationSection<T> configuration)
            where T : class
        {
            return;
        }

        public void Set(IEnumerable<(Guid valueGuid, object? value)> values)
        {
            return;
        }

        [GenericTypeAcceptsConfigurationSection(0)]
        public Task<IConfigurationSection<T>> GetAsync<T>()
            where T : class
        {
            return Task.FromResult((IConfigurationSection<T>)new ConfigurationSection<T>());
        }

        [GenericTypeAcceptsConfigurationSection(0)]
        public Task SetAsync<T>(IConfigurationSection<T> configuration)
            where T : class
        {
            return Task.CompletedTask;
        }

        public Task SetAsync(Guid valueGuid, object? value)
        {
            return Task.CompletedTask;
        }

        public Task SetAsync(IEnumerable<(Guid valueGuid, object? value)> values)
        {
            return Task.CompletedTask;
        }
    }
}

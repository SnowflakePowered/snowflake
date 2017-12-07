using Snowflake.Extensibility.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Configuration;

namespace Snowflake.Extensibility.Provisioning.Standalone
{
    internal class EmptyPluginConfigurationStore : IPluginConfigurationStore
    {
        private static EmptyPluginConfigurationStore emptyPluginConfigurationStore = new EmptyPluginConfigurationStore();

        public static EmptyPluginConfigurationStore EmptyConfigurationStore
            { get => emptyPluginConfigurationStore; }

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
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Snowflake.Extensibility.Configuration;

namespace Snowflake.Extensibility.Provisioning.Standalone
{
    public class StandalonePluginProvision : IPluginProvision
    {
        public ILogger Logger => throw new NotImplementedException();

        public IPluginProperties Properties { get; }

        public IPluginConfigurationStore ConfigurationStore { get; }

        public string Name { get; }

        public string Author { get; }

        public string Description { get; }

        public Version Version { get; }

        public DirectoryInfo ContentDirectory => throw new NotImplementedException();

        public DirectoryInfo ResourceDirectory => throw new NotImplementedException();

        public DirectoryInfo CommonResourceDirectory => throw new NotImplementedException();

        public StandalonePluginProvision(Type pluginType)
            : this(pluginType, EmptyPluginProperties.EmptyProperties)
        {
        }

        public StandalonePluginProvision(Type pluginType, IPluginProperties pluginProperties)
        {
            var attribute = pluginType.GetCustomAttribute<PluginAttribute>();
            if (attribute == null)
            {
                throw new InvalidOperationException("Plugin is not marked with an attribute");
            }

            this.Name = attribute.PluginName;
            this.Author = attribute.Author;
            this.Description = attribute.Description;
            this.Version = attribute.Version;
            this.Properties = pluginProperties;
            this.ConfigurationStore = EmptyPluginConfigurationStore.EmptyConfigurationStore;
        }
    }
}

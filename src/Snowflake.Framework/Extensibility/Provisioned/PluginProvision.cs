using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Snowflake.Extensibility.Configuration;

namespace Snowflake.Extensibility.Provisioned
{
    public class PluginProvision : IPluginProvision
    {
        public PluginProvision(ILogger logger,
            IPluginProperties pluginProperties,
            IPluginConfigurationStore configurationStore,
            string name,
            string author,
            string description,
            Version version,
            DirectoryInfo contentDirectory, DirectoryInfo commonResourceDirectory, DirectoryInfo resourceDirectory)
        {
            this.Logger = logger;
            this.Properties = pluginProperties;
            this.Name = name;
            this.Author = author;
            this.Description = description;
            this.Version = version;
            this.ConfigurationStore = configurationStore;
            this.ContentDirectory = contentDirectory;
            this.ResourceDirectory = resourceDirectory;
            this.CommonResourceDirectory = commonResourceDirectory;
        }

        /// <inheritdoc/>
        public ILogger Logger { get; }

        /// <inheritdoc/>
        public IPluginProperties Properties { get; }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public string Description { get; }

        /// <inheritdoc/>
        public string Author { get; }

        /// <inheritdoc/>
        public Version Version { get; }

        /// <inheritdoc/>
        public DirectoryInfo ContentDirectory { get; }

        /// <inheritdoc/>
        public DirectoryInfo ResourceDirectory { get; }

        /// <inheritdoc/>
        public DirectoryInfo CommonResourceDirectory { get; }

        /// <inheritdoc/>
        public IPluginConfigurationStore ConfigurationStore { get; }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Extensibility.Provisioned
{
    public class PluginProvision : IPluginProvision
    {
        public PluginProvision(ILogger logger,
            IPluginProperties pluginProperties, 
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
            this.ContentDirectory = contentDirectory;
            this.ResourceDirectory = resourceDirectory;
            this.CommonResourceDirectory = commonResourceDirectory;
        }

        public ILogger Logger { get; }

        public IPluginProperties Properties { get; }

        public string Name { get; }

        public string Description { get; }

        public string Author { get; }

        public Version Version { get; }

        public DirectoryInfo ContentDirectory { get; }

        public DirectoryInfo ResourceDirectory { get; }
        
        public DirectoryInfo CommonResourceDirectory { get; }

    }
}

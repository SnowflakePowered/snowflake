using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Extensibility
{
    public class PluginProvision : IPluginProvision
    {
        public PluginProvision(ILogger logger, IPluginProperties pluginProperties, string name, 
            DirectoryInfo contentDirectory, DirectoryInfo commonResourceDirectory, DirectoryInfo resourceDirectory)
        {
            this.Logger = logger;
            this.Properties = pluginProperties;
            this.Name = name;
            this.ContentDirectory = contentDirectory;
            this.ResourceDirectory = resourceDirectory;
            this.CommonResourceDirectory = commonResourceDirectory;
        }

        public ILogger Logger { get; }

        public IPluginProperties Properties { get; }

        public string Name { get; }

        public DirectoryInfo ContentDirectory { get; }

        public DirectoryInfo ResourceDirectory { get; }
        
        public DirectoryInfo CommonResourceDirectory { get; }
    }
}
